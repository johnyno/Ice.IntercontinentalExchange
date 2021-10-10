using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FluentValidation;
using IntercontinentalExchange.Domain.MediatR;
using IntercontinentalExchange.Domain.MediatR.Pipeline;
using MediatR;

namespace IntercontinentalExchange.Domain
{
    public static class DI
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();

        public static ContainerBuilder RegisterDomain(this ContainerBuilder builder, List<Assembly> assemblys)
        {


            builder.RegisterAssemblyTypes(Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            ////services.AddMediatR(Assembly, Application.DI.Assembly, Domain.DI.Assembly);
            builder.RegisterMediatR(assemblys.ToArray());
            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(InputValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(BusinessValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RetryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(PerformanceBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            return builder;
        }
    }
}
