using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Exceptions;
using MediatR;

namespace IntercontinentalExchange.Domain.MediatR.Pipeline
{
    internal class InputValidationBehaviour<TRequest, TResponse> : ValidationBehaviourBase<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        public InputValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            : base(validators.OfType<InputAbstractValidator<TRequest>>())
        {

        }


        protected override void ThrowException(List<ValidationFailure> failures)
        {
            throw new InputValidationException(failures);
        }

    }
}