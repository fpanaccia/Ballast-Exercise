using Aircraft.Api.Application.Services;
using Aircraft.Domain.Aggregates.Airplane;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Aircraft.UnitTests.Application.Services
{
    public class AirplaneServiceTests
    {
        [Fact]
        public async Task CreateAsync_ValidAirplane_Should_ReturnCreatedAirplane()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var airplaneToCreate = new Airplane(Guid.NewGuid(), "B737-800", "41140 kg", "Boeing");

            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Airplane>(), default))
                         .ReturnsAsync(new ValidationResult());

            mockRepository.Setup(r => r.AddAsync(It.IsAny<Airplane>()))
                          .ReturnsAsync(airplaneToCreate);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var createdAirplane = await service.CreateAsync(airplaneToCreate);

            Assert.NotNull(createdAirplane);
            Assert.Equal(airplaneToCreate.Id, createdAirplane.Id);
            Assert.Equal(airplaneToCreate.Model, createdAirplane.Model);
            Assert.Equal(airplaneToCreate.Weight, createdAirplane.Weight);
            Assert.Equal(airplaneToCreate.Manufacturer, createdAirplane.Manufacturer);
        }

        [Fact]
        public async Task CreateAsync_InvalidAirplane_Should_ThrowValidationException()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var invalidAirplane = new Airplane(Guid.NewGuid(), "", "41140 kg", "Boeing");

            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Airplane>(), default))
                         .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                         {
                            new ValidationFailure("Model", "Model is required")
                         }));

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);

            await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(invalidAirplane));
        }

        [Fact]
        public async Task UpdateAsync_InvalidAirplane_Should_ThrowValidationException()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var existingAirplane = new Airplane(Guid.NewGuid(), "B747-100", "187000 kg", "Boeing");
            var invalidAirplane = new Airplane(Guid.NewGuid(), "", "187000 kg", "Boeing");

            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Airplane>(), default))
                         .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                         {
                            new ValidationFailure("Model", "Model is required")
                         }));

            mockRepository.Setup(r => r.FindByIdAsync(existingAirplane.Id))
                          .ReturnsAsync(existingAirplane);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);

            await Assert.ThrowsAsync<ValidationException>(() => service.UpdateAsync(invalidAirplane));
        }

        [Fact]
        public async Task UpdateAsync_ExistingAirplane_Should_ReturnUpdatedAirplane()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var existingAirplane = new Airplane(Guid.NewGuid(), "B747-100", "187000 kg", "Boeing");

            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Airplane>(), default))
                         .ReturnsAsync(new ValidationResult());

            mockRepository.Setup(r => r.FindByIdAsync(existingAirplane.Id))
                          .ReturnsAsync(existingAirplane);

            mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Airplane>()))
                          .ReturnsAsync(existingAirplane);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var updatedAirplane = await service.UpdateAsync(existingAirplane);

            Assert.NotNull(updatedAirplane);
            Assert.Equal(existingAirplane.Id, updatedAirplane.Id);
            Assert.Equal(existingAirplane.Model, updatedAirplane.Model);
            Assert.Equal(existingAirplane.Weight, updatedAirplane.Weight);
            Assert.Equal(existingAirplane.Manufacturer, updatedAirplane.Manufacturer);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingAirplane_Should_ReturnNull()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var nonExistingAirplaneId = Guid.NewGuid();

            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Airplane>(), default))
                         .ReturnsAsync(new ValidationResult());

            mockRepository.Setup(r => r.FindByIdAsync(nonExistingAirplaneId))
                          .ReturnsAsync((Airplane?)null);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var updatedAirplane = await service.UpdateAsync(new Airplane(nonExistingAirplaneId, "B747-100", "187000 kg", "Boeing"));

            Assert.Null(updatedAirplane);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAirplane_Should_DeleteSuccessfully()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var existingAirplaneId = Guid.NewGuid();

            mockRepository.Setup(r => r.FindByIdAsync(existingAirplaneId))
                          .ReturnsAsync(new Airplane(existingAirplaneId, "B747-100", "187000 kg", "Boeing"));

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            await service.DeletAsync(existingAirplaneId);

            mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Airplane>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingAirplane_Should_NotThrow()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var nonExistingAirplaneId = Guid.NewGuid();

            mockRepository.Setup(r => r.FindByIdAsync(nonExistingAirplaneId))
                          .ReturnsAsync((Airplane?)null);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            await service.DeletAsync(nonExistingAirplaneId);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingAirplaneId_Should_ReturnAirplane()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var existingAirplaneId = Guid.NewGuid();
            var existingAirplane = new Airplane(existingAirplaneId, "B747-100", "187000 kg", "Boeing");

            mockRepository.Setup(r => r.FindByIdAsync(existingAirplaneId))
                          .ReturnsAsync(existingAirplane);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var result = await service.GetByIdAsync(existingAirplaneId);

            Assert.NotNull(result);
            Assert.Equal(existingAirplane.Id, result.Id);
            Assert.Equal(existingAirplane.Model, result.Model);
            Assert.Equal(existingAirplane.Weight, result.Weight);
            Assert.Equal(existingAirplane.Manufacturer, result.Manufacturer);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingAirplaneId_Should_ReturnNull()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var nonExistingAirplaneId = Guid.NewGuid();

            mockRepository.Setup(r => r.FindByIdAsync(nonExistingAirplaneId))
                          .ReturnsAsync((Airplane?)null);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var result = await service.GetByIdAsync(nonExistingAirplaneId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnListOfAirplanes()
        {
            var mockRepository = new Mock<IAirplaneRepository>();
            var mockValidator = new Mock<IValidator<Airplane>>();

            var airplanes = new List<Airplane>
            {
                new Airplane(Guid.NewGuid(), "B737-800", "41140 kg", "Boeing"),
                new Airplane(Guid.NewGuid(), "A320", "140000 kg", "Airbus"),
                new Airplane(Guid.NewGuid(), "B747-100", "187000 kg", "Boeing")
            };

            mockRepository.Setup(r => r.GetAllAsync())
                          .ReturnsAsync(airplanes);

            var service = new AirplaneService(mockRepository.Object, mockValidator.Object);
            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(airplanes.Count, result.Count);
            Assert.Equal(airplanes[0].Id, result[0].Id);
            Assert.Equal(airplanes[1].Model, result[1].Model);
            Assert.Equal(airplanes[2].Weight, result[2].Weight);
            Assert.Equal(airplanes[2].Manufacturer, result[2].Manufacturer);
        }
    }
}
