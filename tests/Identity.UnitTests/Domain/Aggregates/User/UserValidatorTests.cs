using FluentValidation.TestHelper;
using Identity.Domain.Aggregates.User;

namespace Identity.UnitTests.Domain.Aggregates.User
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator;

        public UserValidatorTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "", "jdoe@gmail.com", "password");
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Name).WithErrorMessage("Name is required");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenNameIsSpecified()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "jdoe@gmail.com", "password");
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsEmpty()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "jdoe@gmail.com", "");
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Password).WithErrorMessage("Password is required");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenPasswordIsSpecified()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "jdoe@gmail.com", "password");
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsEmpty()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "", "password");
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email).WithErrorMessage("Email is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "email", "password");
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email).WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenEmailIsValid()
        {
            var user = new Identity.Domain.Aggregates.User.User(Guid.NewGuid(), "John Doe", "jdoe@gmail.com", "password");
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Email);
        }
    }
}
