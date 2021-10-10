using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace IntercontinentalExchange.Domain.MediatR
{
    internal class MediatRModule : Module
    {
        private readonly Assembly[] _assemblies;

        public MediatRModule(Assembly[] assemblies/*, Type[] customBehaviorTypes*/)
        {
            _assemblies = assemblies;
            
        }

         
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var openHandlerTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var openHandlerType in openHandlerTypes)
            {
                builder.RegisterAssemblyTypes(_assemblies).AsClosedTypesOf(openHandlerType);
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));


           

            builder.Register<ServiceFactory>(outerContext =>
            {
                var innerContext = outerContext.Resolve<IComponentContext>();
                return serviceType => innerContext.Resolve(serviceType);
            });


        }
    }
}
