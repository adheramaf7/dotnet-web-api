using FluentValidation;
using WebApi.Application.DTOs.Request;

namespace WebApi.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {

        public RegisterRequestValidator()
        {
            RuleFor(field => field.FullName)
                .NotEmpty();

            RuleFor(field => field.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(field => field.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}