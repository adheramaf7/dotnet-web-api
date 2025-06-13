using FluentValidation;
using WebApi.Application.DTOs.Request;

namespace WebApi.Application.Validators.Auth
{
    public class UpdateProfileRequestValidator : AbstractValidator<RegisterRequest>
    {

        public UpdateProfileRequestValidator()
        {
            RuleFor(field => field.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(field => field.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.");
        }
    }
}