using FluentValidation;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;

namespace XUCore.Template.Ddd.Applaction.AppServices.Upload
{
    public class Base64Command : Command<bool>
    {
        public string Base64 { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<Base64Command>
        {
            public Validator()
            {
                RuleFor(x => x.Base64).NotEmpty().WithName("Base64");
            }
        }
    }
}
