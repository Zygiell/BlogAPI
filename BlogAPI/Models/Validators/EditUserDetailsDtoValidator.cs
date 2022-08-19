using FluentValidation;

namespace BlogAPI.Models.Validators
{
    public class EditUserDetailsDtoValidator : AbstractValidator<EditUserDetailsDto>
    {
        public EditUserDetailsDtoValidator()
        {
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(p => p.Password.Length > 0);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);
        }
    }
}