using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Handlers;

namespace IntercontinentalExchange.Domain.Validation
{
    

    public class CoordinatesValidator : InputAbstractValidator<ParseFileHandlerRequest>
    {
        public override ValidationResult Validate(ValidationContext<ParseFileHandlerRequest> context)
        {
            RuleFor(context => context).Must(
                    (context) => context.Lat >= -90 && context.Lat <= 90)
                .WithMessage($"Latitude must be in range [-90, 90]");
            RuleFor(context => context).Must(
                    (context) => context.Lon >= -180 && context.Lon <= 180)
                .WithMessage($"Longitude  must be in range [-180, 180]");

            return base.Validate(context);
        }
    }
}
