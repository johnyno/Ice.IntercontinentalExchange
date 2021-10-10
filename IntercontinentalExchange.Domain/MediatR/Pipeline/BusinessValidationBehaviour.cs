using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Exceptions;
using MediatR;

namespace IntercontinentalExchange.Domain.MediatR.Pipeline
{
    internal class BusinessValidationBehaviour<TRequest, TResponse> : ValidationBehaviourBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        public BusinessValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            : base(validators.OfType<BusinessRuleAbstractValidator<TRequest>>())
        {

        }


        protected override void ThrowException(List<ValidationFailure> failures)
        {
            throw new BusinessRuleValidationException(failures);
        }
    }
}