using HealthMed.API.Controllers;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HealthMed.Tests.Controllers
{
    public class PacienteControllerTests
    {
        private readonly Mock<IPacienteService> _mockService;
        private readonly PacienteController _controller;

        public PacienteControllerTests()
        {
            _mockService = new Mock<IPacienteService>();
            _controller = new PacienteController(_mockService.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new PacienteLoginDTO
            {
                CpfOrEmail = "test@example.com",
                Senha = "password"
            };

            _mockService
                .Setup(service => service.LoginAsync(loginRequest.CpfOrEmail, loginRequest.Senha))
                .ReturnsAsync(new OperationResult<string>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Login realizado com sucesso.",
                    ResultObject = "token"
                });

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(okResult.Value);
            Assert.Equal("Login realizado com sucesso.", response.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenLoginFails()
        {
            // Arrange
            var loginRequest = new PacienteLoginDTO
            {
                CpfOrEmail = "test@example.com",
                Senha = "wrongpassword"
            };

            _mockService
                .Setup(service => service.LoginAsync(loginRequest.CpfOrEmail, loginRequest.Senha))
                .ReturnsAsync(new OperationResult<string>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "CPF ou senha inválidos.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(badRequestResult.Value);
            Assert.Equal("CPF ou senha inválidos.", response.Message);
        }

        [Fact]
        public async Task Cadastrar_ShouldReturnOk_WhenCadastroIsSuccessful()
        {
            // Arrange
            var paciente = new PacienteCadastroDTO
            {
                Cpf = "12345678900",
                Email = "test@example.com",
                Senha = "password"
            };

            _mockService
                .Setup(service => service.CadastrarAsync(paciente))
                .ReturnsAsync(new OperationResult<string>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Paciente cadastrado com sucesso.",
                    ResultObject = "PacienteId"
                });

            // Act
            var result = await _controller.Cadastrar(paciente);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(okResult.Value);
            Assert.Equal("Paciente cadastrado com sucesso.", response.Message);
        }

        [Fact]
        public async Task Cadastrar_ShouldReturnConflict_WhenPacienteAlreadyExists()
        {
            // Arrange
            var paciente = new PacienteCadastroDTO
            {
                Cpf = "12345678900",
                Email = "test@example.com",
                Senha = "password"
            };

            _mockService
                .Setup(service => service.CadastrarAsync(paciente))
                .ReturnsAsync(new OperationResult<string>
                {
                    Status = TypeReturnStatus.Conflict,
                    Message = "Paciente com este CPF ou e-mail já está cadastrado.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.Cadastrar(paciente);

            // Assert
            var conflictResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<string>>(conflictResult.Value);
            Assert.Equal("Paciente com este CPF ou e-mail já está cadastrado.", response.Message);
        }
    }
}
