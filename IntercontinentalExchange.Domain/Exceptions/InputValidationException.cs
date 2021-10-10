using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace IntercontinentalExchange.Domain.Exceptions
{
    public class InputValidationException : ValidationException
    {
        public InputValidationException(string message) : base(message) { }

        public InputValidationException(IEnumerable<ValidationFailure> errors) : base(errors) { }

        public InputValidationException(string message, IEnumerable<ValidationFailure> errors) : base(errors) { }
    }
}
