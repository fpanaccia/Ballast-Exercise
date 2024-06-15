using Aircraft.Domain.Aggregates.Airplane;
using FluentValidation.TestHelper;

namespace Aircraft.UnitTests.Domain.Aggregates.Airplane
{
    public class AirplaneValidatorTests
    {
        private readonly AirplaneValidator _validator;

        public AirplaneValidatorTests()
        {
            _validator = new AirplaneValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenModelIsEmpty()
        {
            var airplane = new Aircraft.Domain.Aggregates.Airplane.Airplane(Guid.NewGuid(), "", "41140 kg", "Boeing");
            var result = _validator.TestValidate(airplane);
            result.ShouldHaveValidationErrorFor(a => a.Model).WithErrorMessage("Model is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenWeightIsEmpty()
        {
            var airplane = new Aircraft.Domain.Aggregates.Airplane.Airplane(Guid.NewGuid(), "B737-800", "", "Boeing");
            var result = _validator.TestValidate(airplane);
            result.ShouldHaveValidationErrorFor(a => a.Weight).WithErrorMessage("Weight is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenManufacturerIsEmpty()
        {
            var airplane = new Aircraft.Domain.Aggregates.Airplane.Airplane(Guid.NewGuid(), "B737-800", "41140 kg", "");
            var result = _validator.TestValidate(airplane);
            result.ShouldHaveValidationErrorFor(a => a.Manufacturer).WithErrorMessage("Manufacturer is required");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Properties_Are_Present()
        {
            var airplane = new Aircraft.Domain.Aggregates.Airplane.Airplane(Guid.NewGuid(), "B737-800", "41140 kg", "Boeing");
            var result = _validator.TestValidate(airplane);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
