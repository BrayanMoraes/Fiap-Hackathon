using HealthMed.API.Controllers;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HealthMed.Tests.Controllers
{
    public class HealthMedBaseControllerTests
    {
        private readonly HealthMedBaseController _controller;

        public HealthMedBaseControllerTests()
        {
            _controller = new HealthMedBaseController();
        }

        [Fact]
        public void TratarRetorno_ShouldReturnOk_WhenStatusIsSuccess()
        {
            // Arrange
            var operationResult = new OperationResult<string>
            {
                Status = TypeReturnStatus.Success,
                Message = "Operação realizada com sucesso.",
                ResultObject = "Resultado"
            };

            // Act
            var result = _controller.TratarRetorno(operationResult);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(okResult.Value);
            Assert.Equal(TypeReturnStatus.Success, response.Status);
            Assert.Equal("Operação realizada com sucesso.", response.Message);
            Assert.Equal("Resultado", response.ResultObject);
        }

        [Fact]
        public void TratarRetorno_ShouldReturnBadRequest_WhenStatusIsError()
        {
            // Arrange
            var operationResult = new OperationResult<string>
            {
                Status = TypeReturnStatus.Error,
                Message = "Ocorreu um erro.",
                ResultObject = null
            };

            // Act
            var result = _controller.TratarRetorno(operationResult);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(badRequestResult.Value);
            Assert.Equal(TypeReturnStatus.Error, response.Status);
            Assert.Equal("Ocorreu um erro.", response.Message);
            Assert.Null(response.ResultObject);
        }

        [Fact]
        public void TratarRetorno_ShouldReturnBadRequest_WhenStatusIsConflict()
        {
            // Arrange
            var operationResult = new OperationResult<string>
            {
                Status = TypeReturnStatus.Conflict,
                Message = "Conflito detectado.",
                ResultObject = null
            };

            // Act
            var result = _controller.TratarRetorno(operationResult);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(badRequestResult.Value);
            Assert.Equal(TypeReturnStatus.Conflict, response.Status);
            Assert.Equal("Conflito detectado.", response.Message);
            Assert.Null(response.ResultObject);
        }

        [Fact]
        public void TratarRetorno_ShouldReturnBadRequest_WhenStatusIsUnknown()
        {
            // Arrange
            var operationResult = new OperationResult<string>
            {
                Status = (TypeReturnStatus)999, // Status desconhecido
                Message = "Status desconhecido.",
                ResultObject = null
            };

            // Act
            var result = _controller.TratarRetorno(operationResult);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(badRequestResult.Value);
            Assert.Equal((TypeReturnStatus)999, response.Status);
            Assert.Equal("Status desconhecido.", response.Message);
            Assert.Null(response.ResultObject);
        }
    }
}

