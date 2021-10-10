using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Exceptions;
using MediatR;

namespace IntercontinentalExchange.Domain.Bases
{
    internal abstract class ValidationBehaviourBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;


        protected ValidationBehaviourBase(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (_validators.Any())
            {
                var tasks = _validators
                    .Select(v => Task.Factory.StartNew(() =>
                    {
                        var context = new ValidationContext<TRequest>(request);
                        return v.Validate(context);
                    }, cancellationToken));

                var businessRuleValidationResults = await Task.WhenAll(tasks);

                var businessRuleFailures = businessRuleValidationResults
                    .SelectMany(r => r.Errors).Where(f => f != null)
                    .Distinct()
                    .ToList();

                if (businessRuleFailures.Count != 0)
                {
                    throw new BusinessRuleValidationException(businessRuleFailures);
                }
            }

            return await next();
        }

        protected abstract void ThrowException(List<ValidationFailure> failures);
    }
}
