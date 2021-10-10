using System.Reflection;
using Autofac;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Contracts.Configurations;
using IntercontinentalExchange.Infrastructure.Services;
using IntercontinentalExchange.Infrastructure.Services.Configurations;

namespace IntercontinentalExchange.Infrastructure
{
    public static class DI
    {
        public static Assembly Assembly => Assembly.GetExecutingAssembly();

        public static ContainerBuilder RegisterInfrastructure(this ContainerBuilder builder)
        {
            builder.RegisterType<AppLogger>().As<IAppLogger>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadNoaaFileService>().As<IDownloadNoaaFilesService>().SingleInstance();
            builder.RegisterType<DownloadFileService>().As<IDownloadFileService>().SingleInstance();
            builder.RegisterType<ParseNoaaFilesService>().As<IParseNoaaFilesService>().SingleInstance();



            builder.RegisterType<DownloadNoaaFileServiceConfig>().As<IDownloadNoaaFileServiceConfig>().SingleInstance();
            builder.RegisterType<ParseNoaaFilesServiceConfig>().As<IParseNoaaFilesServiceConfig>().SingleInstance();
            return builder;
        }
    }
}
