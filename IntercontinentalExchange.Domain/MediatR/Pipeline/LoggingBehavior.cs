using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IntercontinentalExchange.Domain.Contracts;
using MediatR;

namespace IntercontinentalExchange.Domain.MediatR.Pipeline
{
    internal class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAppLogger _logger;

        public LoggingBehaviour(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                _logger.Info($"Handling {requestName}");
                //LogRequestProps(request);
                var output = await next();
                _logger.Success($"{requestName} execution success");
                return output;
            }
            catch (ValidationException ex)
            {
                if (!ex.Errors.Any())
                {
                    _logger.ValidationError(ex.Message);
                }

                foreach (var failure in ex.Errors.Select(c => new { c.PropertyName, c.ErrorMessage }).Distinct().ToList())
                {
                    _logger.ValidationError("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
                throw;
            }
        }
    }
}
