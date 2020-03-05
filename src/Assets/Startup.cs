using System.Reflection;
using Assets.Configuration;
using Assets.GrpcServices;
using Assets.Repositories.Context;
using Autofac;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swisschain.Sdk.Server.Common;

namespace Assets
{
    public sealed class Startup : SwisschainStartup<AppConfig>
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            base.RegisterEndpoints(endpoints);

            endpoints.MapGrpcService<AssetsService>();
            endpoints.MapGrpcService<AssetPairsService>();
            endpoints.MapGrpcService<MonitoringService>();
        }

        protected override void ConfigureServicesExt(IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(AutoMapperProfile), typeof(Repositories.AutoMapperProfile))
                .AddControllersWithViews()
                .AddFluentValidation(options =>
                {
                    ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
                    options.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly());
                });
        }

        protected override void ConfigureContainerExt(ContainerBuilder builder)
        {
            builder.RegisterModule(new Services.AutofacModule());
            builder.RegisterModule(new Repositories.AutofacModule(Config.AssetsService.Db.DataConnectionString));
        }

        protected override void ConfigureExt(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetRequiredService<AutoMapper.IConfigurationProvider>()
                .AssertConfigurationIsValid();

            app.ApplicationServices.GetRequiredService<ConnectionFactory>()
                .EnsureMigration();
        }
    }
}
