using FluentValidation;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;

namespace Identity.Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;

        public UserService(IUserRepository userRepository, IValidator<User> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<User> CreateAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userOnDb = await _userRepository.FindByEmailAsync(user.Email);
            if (userOnDb != null)
            {
                throw new UserAlreadyExistException();
            }

            //TODO we should encrypt the password, but for this exercise, it is not necessary

            await _userRepository.AddAsync(user);

            //TODO we should send a event to send the email to "activate" the account

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
