using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using FluentValidation;

namespace IntercontinentalExchange.Application
{
    public static class DI
    {
        public static Assembly Assembly => Assembly.GetExecutingAssembly();

        public static ContainerBuilder RegisterApplication(this ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(Assembly)
            //.AsImplementedInterfaces()
            //.InstancePerDependency();
            //builder.RegisterType<HcHostedService>().As<IHcHostedService>().InstancePerDependency();
            //builder.RegisterType<AppStartupValidationService>().As<IAppStartupValidationService>().InstancePerDependency();

            //builder.RegisterAssemblyTypes(Assembly, Queries.DI.Assembly, Commands.DI.Assembly)
            //    .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();

            //builder.RegisterType<HcHostedService>().InstancePerDependency();
            return builder;
        }
    }
}
