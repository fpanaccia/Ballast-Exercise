using FluentValidation;

namespace Identity.Domain.Aggregates.User
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required")
                                       .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
