using HealthMed.API.Controllers;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HealthMed.Tests.Controllers
{
    public class MedicoControllerTests
    {
        private readonly Mock<IMedicoService> _mockService;
        private readonly MedicoController _controller;

        public MedicoControllerTests()
        {
            _mockService = new Mock<IMedicoService>();
            _controller = new MedicoController(_mockService.Object);
        }

        [Fact]
        public async Task Cadastrar_ShouldReturnOk_WhenCadastroIsSuccessful()
        {
            // Arrange
            var medicoCadastro = new MedicoCadastroDTO
            {
                CRM = "123456",
                NomeCompleto = "Dr. Teste",
                IdEspecialidade = Guid.NewGuid(),
                Senha = "password"
            };

            _mockService
                .Setup(service => service.CadastroMedico(medicoCadastro))
                .ReturnsAsync(new OperationResult<Guid?>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Medico cadastrado com Sucesso.",
                    ResultObject = Guid.NewGuid()
                });

            // Act
            var result = await _controller.Cadastrar(medicoCadastro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<Guid?>>(okResult.Value);
            Assert.Equal(TypeReturnStatus.Success, response.Status);
            Assert.Equal("Medico cadastrado com Sucesso.", response.Message);
        }

        [Fact]
        public async Task Cadastrar_ShouldReturnConflict_WhenMedicoAlreadyExists()
        {
            // Arrange
            var medicoCadastro = new MedicoCadastroDTO
            {
                CRM = "123456",
                NomeCompleto = "Dr. Teste",
                IdEspecialidade = Guid.NewGuid(),
                Senha = "password"
            };

            _mockService
                .Setup(service => service.CadastroMedico(medicoCadastro))
                .ReturnsAsync(new OperationResult<Guid?>
                {
                    Status = TypeReturnStatus.Conflict,
                    Message = "Já existe um médico cadastrado com este CRM.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.Cadastrar(medicoCadastro);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<Guid?>>(badRequestResult.Value);
            Assert.Equal(TypeReturnStatus.Conflict, response.Status);
            Assert.Equal("Já existe um médico cadastrado com este CRM.", response.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new MedicoLoginDTO
            {
                CRM = "123456",
                Senha = "password"
            };

            var loginResponse = new MedicoLoginRetornoDTO
            {
                MedicoId = Guid.NewGuid(),
                NomeMedico = "Dr. Teste",
                Token = "jwt-token"
            };

            _mockService
                .Setup(service => service.Login(loginRequest))
                .ReturnsAsync(new OperationResult<MedicoLoginRetornoDTO?>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Login realizado com sucesso.",
                    ResultObject = loginResponse
                });

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<MedicoLoginRetornoDTO?>>(okResult.Value);
            Assert.Equal(TypeReturnStatus.Success, response.Status);
            Assert.Equal("Login realizado com sucesso.", response.Message);
            Assert.NotNull(response.ResultObject);
            Assert.Equal("Dr. Teste", response.ResultObject.NomeMedico);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenLoginFails()
        {
            // Arrange
            var loginRequest = new MedicoLoginDTO
            {
                CRM = "123456",
                Senha = "wrongpassword"
            };

            _mockService
                .Setup(service => service.Login(loginRequest))
                .ReturnsAsync(new OperationResult<MedicoLoginRetornoDTO?>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "CRM ou Senha inválidos.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<MedicoLoginRetornoDTO?>>(badRequestResult.Value);
            Assert.Equal(TypeReturnStatus.Error, response.Status);
            Assert.Equal("CRM ou Senha inválidos.", response.Message);
        }
    }
}

