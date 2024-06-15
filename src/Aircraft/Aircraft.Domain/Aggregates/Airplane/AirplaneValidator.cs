using FluentValidation;

namespace Aircraft.Domain.Aggregates.Airplane
{
    public class AirplaneValidator : AbstractValidator<Airplane>
    {
        public AirplaneValidator()
        {
            RuleFor(user => user.Model).NotEmpty().WithMessage("Model is required");
            RuleFor(user => user.Weight).NotEmpty().WithMessage("Weight is required");
            RuleFor(user => user.Manufacturer).NotEmpty().WithMessage("Manufacturer is required");
        }
    }
}
