using Aircraft.Api.Application.Commands;
using Aircraft.Api.Controllers;
using Aircraft.Domain.Aggregates.Airplane;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Aircraft.UnitTests.Controllers
{
    public class AirplaneControllerTests
    {
        private readonly Mock<IAirplaneService> _mockAirplaneService;
        private readonly AirplaneController _controller;

        public AirplaneControllerTests()
        {
            _mockAirplaneService = new Mock<IAirplaneService>();
            _controller = new AirplaneController(_mockAirplaneService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfAirplanes()
        {
            var expectedAirplanes = new List<Airplane>
            {
                new Airplane(Guid.NewGuid(), "B737-800", "41140 Kg", "Boeing"),
                new Airplane(Guid.NewGuid(), "A320", "37400 Kg", "Airbus")
            };

            _mockAirplaneService.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(expectedAirplanes);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAirplanes = Assert.IsAssignableFrom<List<Airplane>>(okResult.Value);
            Assert.Equal(expectedAirplanes.Count, actualAirplanes.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WitAirplane()
        {
            var id = Guid.NewGuid();
            var expectedAirplane = new Airplane(id, "B737-800", "41140 Kg", "Boeing");

            _mockAirplaneService.Setup(repo => repo.GetByIdAsync(id))
                                .ReturnsAsync(expectedAirplane);

            var result = await _controller.Get(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAirplane = Assert.IsType<Airplane>(okResult.Value);

            Assert.Equal(expectedAirplane.Id, actualAirplane.Id);
            Assert.Equal(expectedAirplane.Model, actualAirplane.Model);
            Assert.Equal(expectedAirplane.Weight, actualAirplane.Weight);
            Assert.Equal(expectedAirplane.Manufacturer, actualAirplane.Manufacturer);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenAirplaneDoesNotExist()
        {
            var id = Guid.NewGuid();

            _mockAirplaneService.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Airplane?)null);

            var result = await _controller.Get(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WitUpdatedAirplane()
        {
            var id = Guid.NewGuid();
            var updateModel = new UpdateAirplane { Id = id, Model = "B737-800", Weight = "41140 Kg", Manufacturer = "Boeing" };
            var updatedAirplane = new Airplane(updateModel.Id, updateModel.Model, updateModel.Weight, updateModel.Manufacturer);

            _mockAirplaneService.Setup(repo => repo.UpdateAsync(It.IsAny<Airplane>()))
                                .ReturnsAsync(updatedAirplane);

            var result = await _controller.Put(id, updateModel);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAirplane = Assert.IsType<Airplane>(okResult.Value);
            Assert.Equal(updatedAirplane.Id, actualAirplane.Id);
            Assert.Equal(updatedAirplane.Model, actualAirplane.Model);
            Assert.Equal(updatedAirplane.Weight, actualAirplane.Weight);
            Assert.Equal(updatedAirplane.Manufacturer, actualAirplane.Manufacturer);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenInvalidId()
        {
            var id = Guid.NewGuid();
            var updateModel = new UpdateAirplane { Id = Guid.NewGuid(), Model = "B737-800", Weight = "41140 Kg", Manufacturer = "Boeing" };

            var result = await _controller.Put(id, updateModel);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("ID does not correspond with id in object", badRequestResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnNotFound_WhenAirplaneDoesNotExist()
        {
            var id = Guid.NewGuid();
            var updateModel = new UpdateAirplane { Id = id, Model = "B737-800", Weight = "41140 Kg", Manufacturer = "Boeing" };

            _mockAirplaneService.Setup(repo => repo.UpdateAsync(It.IsAny<Airplane>())).ReturnsAsync((Airplane?)null);

            var result = await _controller.Put(id, updateModel);
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenModelIsInvalidt()
        {
            var id = Guid.NewGuid();
            var updateModel = new UpdateAirplane { Id = id, Model = "", Weight = "41140 Kg", Manufacturer = "Boeing" };
            var validationFailure = new ValidationFailure("Model", "Model is required");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _mockAirplaneService.Setup(repo => repo.UpdateAsync(It.IsAny<Airplane>())).ThrowsAsync(new ValidationException(validationResult.Errors));

            var result = await _controller.Put(id, updateModel);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenExistingId()
        {
            var id = Guid.NewGuid();
            var airplaneToDelete = new Airplane(id, "B737-800", "41140 Kg", "Boeing");

            _mockAirplaneService.Setup(repo => repo.GetByIdAsync(id))
                                .ReturnsAsync(airplaneToDelete);

            var result = await _controller.Delete(id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenNonExistingId()
        {
            var id = Guid.NewGuid();
            _mockAirplaneService.Setup(repo => repo.GetByIdAsync(id))
                                .ReturnsAsync((Airplane)null);

            var result = await _controller.Delete(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ShouldReturnCreated_WhenAirplaneIsValid()
        {
            var createModel = new CreateAirplane { Model = "B737-800", Weight = "41140 Kg", Manufacturer = "Boeing" };
            var createdAirplane = new Airplane(Guid.NewGuid(), createModel.Model, createModel.Weight, createModel.Manufacturer);

            _mockAirplaneService.Setup(repo => repo.CreateAsync(It.IsAny<Airplane>()))
                                .ReturnsAsync(createdAirplane);


            var result = await _controller.Post(createModel);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var actualAirplane = Assert.IsType<Airplane>(createdAtActionResult.Value);

            Assert.Equal(createdAirplane.Id, actualAirplane.Id);
            Assert.Equal(createdAirplane.Model, actualAirplane.Model);
            Assert.Equal(createdAirplane.Weight, actualAirplane.Weight);
            Assert.Equal(createdAirplane.Manufacturer, actualAirplane.Manufacturer);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenInvalidModel()
        {
            var createModel = new CreateAirplane { Model = "", Weight = "41140 Kg", Manufacturer = "Boeing" };
            var validationFailure = new ValidationFailure("Model", "Model is required");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _mockAirplaneService.Setup(repo => repo.CreateAsync(It.IsAny<Airplane>()))
                                .ThrowsAsync(new ValidationException(validationResult.Errors));

            var result = await _controller.Post(createModel);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }
    }
}
