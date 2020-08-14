﻿using Mk.DemoC;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(DemoBApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        // typeof(DemoBBackgroundJobsModule),    // 先去掉后台任务，不然hangfire一直打日志

        // 远程调用C，需要依赖远程  C代理Module
        typeof(DemoCHttpApiClientModule)
        )]
    public class DemoBApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DemoBApplicationModule>();
            });
        }
    }
}
