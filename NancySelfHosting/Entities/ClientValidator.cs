
namespace NancySelfHosting.Entities
{
    using FluentValidation;

    public class ClientValidator : AbstractValidator<ClientEntity>
    {
        public ClientValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(m => m.Lastname).NotEmpty().WithMessage("Lastname is required");
            RuleFor(m => m.Address).NotEmpty().WithMessage("Address is required");
        }
    }

}
