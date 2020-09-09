﻿using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Leopard.Results;
using Nest;
using Mk.DemoC.Dto.ElastcSearchs;

namespace Mk.DemoC.ElastcSearchAppService
{
    [Route("api/democ/elastic")]
    public class ElastcSearchAppService : DemoCAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IProductSpuDocRepository _productSpuDocRepository;

        public ElastcSearchAppService(
            ILogger<ElastcSearchAppService> logger
            , IHttpClientFactory clientFactory
            , IProductSpuDocRepository productSpuDocRepository
            )
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _productSpuDocRepository = productSpuDocRepository;
        }

        //https://www.zyccst.com/yaocai-277.html		狗鞭
        //https://www.zyccst.com/yaocai-786.html		广金钱草
        //https://www.zyccst.com/yaocai-192.html		枸杞子
        //https://www.zyccst.com/yaocai-526.html		石斛
        [HttpPost("product/capture")]
        public async Task<ServiceResult<long>> CaptureProductDocAsync()
        {
            ServiceResult<long> ret = new ServiceResult<long>(IdProvider.Get());
            long hadCaptureCount = 0;
            HttpClient client = _clientFactory.CreateClient();

            Dictionary<string, string> urlMap = new Dictionary<string, string>
            {
                { "277","狗鞭"},
                { "786","广金钱草"},
                { "192","枸杞子"},
                { "526","石斛"},
            };
            string urlTemplate = "https://www.zyccst.com/yaocai-{0}.html";
            string subUrlTemplate = "https://www.zyccst.com/yaocai-{0}-{1}.html";
            foreach (var item in urlMap)
            {
                string url = string.Format(urlTemplate, item.Key);
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode node = doc.DocumentNode.SelectSingleNode("//em[@class='n red num']");
                int totalCount = int.Parse(node.InnerText);
                if (totalCount > 0)
                {
                    // 每页12条记录。只获取前五页数据
                    int capturePageCount = ((totalCount / 12) + 1) > 5 ? 5 : ((totalCount / 12) + 1);

                    List<ProductSpuDoc> list = new List<ProductSpuDoc>();
                    for (var i = 1; i <= 5; i++)
                    {
                        string productListUrl = string.Format(subUrlTemplate, item.Key, i);
                        _logger.LogInformation($"产品抓取[{item.Value}],url:[{productListUrl}]");
                        HttpResponseMessage subResponse = await client.GetAsync(productListUrl).ConfigureAwait(false);
                        string subHtml = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        HtmlDocument subDoc = new HtmlDocument();
                        subDoc.LoadHtml(subHtml);

                        HtmlNodeCollection productNodes = doc.DocumentNode.SelectNodes("//div[@class='productUnit clf']");
                        foreach (var product in productNodes)
                        {
                            HtmlNode titleNode = product.SelectSingleNode(".//div[@class='proTitle']").SelectSingleNode(".//a");
                            string productName = titleNode.InnerText;
                            string productUrl = titleNode.Attributes["href"].Value;
                            string productCode = (productUrl.Split("-")).Last().Split(".")[0];
                            decimal price = decimal.Parse(product.SelectSingleNode(".//div[@class='proPrice l']").SelectSingleNode(".//span").InnerText.Split(" ").Last());

                            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), productCode, null
                                , productName, "诚实通", item.Value, null, "CNY", price, price);
                            list.Add(entity);
                            hadCaptureCount++;
                        }

                        list.ForEach(async i => await _productSpuDocRepository.InsertAsync(i));
                        await CurrentUnitOfWork.SaveChangesAsync();
                        list.Clear();
                    }
                }
            }
            ret.SetSuccess(hadCaptureCount);
            return ret;
        }

        [HttpDelete("product/all")]
        public async Task<ServiceResult> DeleteProductDocAsync()
        {
            ServiceResult ret = new ServiceResult(IdProvider.Get());
            await _productSpuDocRepository.DeleteAllAsync();
            ret.SetSuccess();
            return ret;
        }

        // Elasticsearch .Net Client NEST 多条件查询示例
        // https://www.cnblogs.com/huhangfei/p/5985280.html
        // Elasticsearch .net client NEST 5.x 使用总结
        // https://www.cnblogs.com/huhangfei/p/7524886.html
        // Elasticsearch搜索查询语法
        // https://www.cnblogs.com/haixiang/p/12095578.html

        [HttpPost("document/create")]
        public async Task CreateDocumentIndex()
        {
            var node = new Uri("http://134.175.121.78:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            ProductSpuDoc entity = new ProductSpuDoc(GuidGenerator.Create(), "A001", null
                                , "一个产品", "诚实通", "关键词", null, "CNY", 12, 12);
            ProductSpuDocument document = ObjectMapper.Map<ProductSpuDoc, ProductSpuDocument>(entity);

            var response = await client.IndexAsync(document, idx => idx.Index("mall_search"));
           
            var getResponse = await client.GetAsync<ProductSpuDocument>(1, idx => idx.Index("mall_search"));
            var r = getResponse.Source;

            var searchRequest = new SearchRequest("mall_search")
            {
                From = 0,
                Size = 10,
                Query = new TermQuery { Field = "spucode", Value = "A001" }
            };

            var searchResponse = client.Search<ProductSpuDocument>();

        }
    }
}
