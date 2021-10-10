using System.IO;
using FluentValidation;
using FluentValidation.Results;
using IntercontinentalExchange.Domain.Bases;
using IntercontinentalExchange.Domain.Handlers;

namespace IntercontinentalExchange.Domain.Validation
{
    public class ParserExistenceValidator:BusinessRuleAbstractValidator<ParseFileHandlerRequest>
    {
        public override ValidationResult Validate(ValidationContext<ParseFileHandlerRequest> context)
        {
            RuleFor(context => context).Must(
                    (context) => File.Exists(context.ParserLocation))
                .WithMessage("Parser software doesn't exist in noted location");
           
            return base.Validate(context);
        }
    }
}
