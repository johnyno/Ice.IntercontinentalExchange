using System;
using System.Collections.Generic;
using System.Reflection;
using AspNetCoreRateLimit;
using Autofac;
using IntercontinentalExchange.Application;
using IntercontinentalExchange.Domain;
using IntercontinentalExchange.Host.Extntions;
using IntercontinentalExchange.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntercontinentalExchange.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // needed to load configuration from appsettings.json 
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            // inject counter and rules stores
            services.AddInMemoryRateLimiting();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSingleton(Configuration);
            
            services.AddControllers().AddJsonOptions(Setup.ControllersJsonSetupAction);
            
            services.AddSwaggerGen();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            var assemblyList = new List<Assembly>
            {
                Assembly.GetExecutingAssembly(),
                Application.DI.Assembly,
                Domain.DI.Assembly
            };

            builder
                .RegisterApplication()
                .RegisterDomain(assemblyList)
                .RegisterInfrastructure();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseDeveloperExceptionPage()
                .UseExceptionHandler(Setup.ExceptionHandlerSetupAction)
                .UseStaticFiles()
                .UseSwagger().UseSwaggerUI(Setup.SwaggerUiSetupAction)
                .UseRouting()
                .UseIpRateLimiting()
                .UseEndpoints(Setup.EndPointSetupAction);

            Setup.Init(app);
        }

       
    }
}
