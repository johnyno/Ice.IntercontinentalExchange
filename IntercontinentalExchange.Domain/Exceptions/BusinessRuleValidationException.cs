using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace IntercontinentalExchange.Domain.Exceptions
{
    public class BusinessRuleValidationException : ValidationException
    {
        public BusinessRuleValidationException(string message) : base(message) { }

        public BusinessRuleValidationException(IEnumerable<ValidationFailure> errors) : base(errors) { }

        public BusinessRuleValidationException(string message, IEnumerable<ValidationFailure> errors) : base(errors) { }

    }
}
