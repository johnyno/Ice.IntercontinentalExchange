using System;
using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Handlers;

namespace IntercontinentalExchange.Domain.Validation
{
    public class ForecastDateValidator : InputAbstractValidator<DownloadForecastFileHandlerRequest>
    {
        public override ValidationResult Validate(ValidationContext<DownloadForecastFileHandlerRequest> context)
        {
            RuleFor(context => context).Must(
                    (context) => (context.Date - DateTime.UtcNow).TotalHours <=120)
                .WithMessage("Forecast offset is maximum 120 hours");
          

            return base.Validate(context);
        }
    }
}
