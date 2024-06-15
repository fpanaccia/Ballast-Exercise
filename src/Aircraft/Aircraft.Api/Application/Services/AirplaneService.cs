using FluentValidation;
using Aircraft.Domain.Aggregates.Airplane;

namespace Aircraft.Api.Application.Services
{
    public class AirplaneService : IAirplaneService
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IValidator<Airplane> _validator;

        public AirplaneService(IAirplaneRepository airplaneRepository, IValidator<Airplane> validator)
        {
            _airplaneRepository = airplaneRepository;
            _validator = validator;
        }

        public async Task<Airplane> CreateAsync(Airplane airplane)
        {
            var validationResult = await _validator.ValidateAsync(airplane);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _airplaneRepository.AddAsync(airplane);
        }

        public async Task<Airplane?> UpdateAsync(Airplane airplane)
        {
            var validationResult = await _validator.ValidateAsync(airplane);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var airplaneOnDb = await _airplaneRepository.FindByIdAsync(airplane.Id);
            if (airplaneOnDb is null)
            {
                return null;
            }

            return await _airplaneRepository.UpdateAsync(airplane);
        }

        public async Task DeletAsync(Guid airplaneId)
        {
            var airplane = await _airplaneRepository.FindByIdAsync(airplaneId);
            if (airplane != null)
            {
                await _airplaneRepository.DeleteAsync(airplane);
            }
        }

        public async Task<Airplane?> GetByIdAsync(Guid airplaneId)
        {
            return await _airplaneRepository.FindByIdAsync(airplaneId);
        }

        public async Task<List<Airplane>> GetAllAsync()
        {
            return await _airplaneRepository.GetAllAsync();
        }
    }
}
