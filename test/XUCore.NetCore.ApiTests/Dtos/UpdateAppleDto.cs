using FluentValidation;
using XUCore.Ddd.Domain;

namespace XUCore.NetCore.ApiTests.Dtos
{
    public class UpdateAppleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Validator : CommandValidator<UpdateAppleDto>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("id");
            RuleFor(x => x.Name).NotEmpty().WithName("name");
        }
    }
}
