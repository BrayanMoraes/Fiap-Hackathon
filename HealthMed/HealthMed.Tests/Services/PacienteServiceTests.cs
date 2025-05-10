using HealthMed.Business.Services;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using Moq;
using Xunit;

namespace HealthMed.Tests.Services
{
    public class PacienteServiceTests
    {
        private readonly Mock<IPacienteRepository> _mockRepository;
        private readonly PacienteService _service;

        public PacienteServiceTests()
        {
            _mockRepository = new Mock<IPacienteRepository>();
            _service = new PacienteService(_mockRepository.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByCpfOrEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Paciente)null);

            // Act
            var result = await _service.LoginAsync("12345678900", "wrongpassword");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal("CPF ou senha inválidos.", result.Message);
        }

        [Fact]
        public async Task CadastrarAsync_ShouldReturnSuccess_WhenPacienteIsValid()
        {
            // Arrange
            var paciente = new PacienteCadastroDTO
            {
                Cpf = "12345678900",
                Email = "test@example.com",
                Senha = "password"
            };

            _mockRepository.Setup(repo => repo.GetByCpfOrEmailAsync(paciente.Cpf, paciente.Email)).ReturnsAsync((Paciente)null);

            // Act
            var result = await _service.CadastrarAsync(paciente);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Success, result.Status);
            Assert.Equal("Paciente cadastrado com sucesso.", result.Message);
        }

        [Fact]
        public async Task CadastrarAsync_ShouldReturnError_WhenPacienteAlreadyExists()
        {
            // Arrange
            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                CPF = "12345678900",
                Email = "test@example.com",
                Senha = "password"
            };

            var pacienteDTO = new PacienteCadastroDTO
            {
                Cpf = "12345678900",
                Email = "test@example.com",
                Senha = "password"
            };

            _mockRepository.Setup(repo => repo.GetByCpfOrEmailAsync(paciente.CPF, paciente.Email)).ReturnsAsync(paciente);

            // Act
            var result = await _service.CadastrarAsync(pacienteDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Conflict, result.Status);
            Assert.Equal("Paciente com este CPF ou e-mail já está cadastrado.", result.Message);
        }
    }
}


