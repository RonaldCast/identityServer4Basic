using FluentValidation;
using Identity_server.DTO;

namespace Identity_server.Validator
{
    public class ProofValidate : AbstractValidator<Proof>
    {
        public ProofValidate()
        {
            RuleFor(a => a.Name)
                .NotEmpty().MinimumLength(50).WithMessage("Debe ser mayor o igual a 50");
        }
    }
}