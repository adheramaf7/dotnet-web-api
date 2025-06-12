using FluentValidation;
using WebApi.Application.DTOs.Request;

namespace WebApi.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {

        public RegisterRequestValidator()
        {
            RuleFor(field => field.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(field => field.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(field => field.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}