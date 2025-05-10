using HealthMed.Business.Services;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using Moq;
using Xunit;

namespace HealthMed.Tests.Services
{
    public class MedicoServiceTests
    {
        private readonly Mock<IMedicoRepository> _mockRepository;
        private readonly MedicoService _service;

        public MedicoServiceTests()
        {
            _mockRepository = new Mock<IMedicoRepository>();
            _service = new MedicoService(_mockRepository.Object);
        }

        [Fact]
        public async Task CadastroMedico_ShouldReturnSuccess_WhenMedicoIsValid()
        {
            // Arrange
            var medicoDto = new MedicoCadastroDTO
            {
                CRM = "123456",
                NomeCompleto = "Dr. Teste",
                IdEspecialidade = Guid.NewGuid(),
                Senha = "password"
            };

            _mockRepository.Setup(repo => repo.GetByCRMAsync(It.IsAny<string>())).ReturnsAsync((Medico)null);

            // Act
            var result = await _service.CadastroMedico(medicoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Success, result.Status);
            Assert.Equal("Medico cadastrado com Sucesso", result.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnError_WhenMedicoDoesNotExist()
        {
            // Arrange
            var loginDto = new MedicoLoginDTO
            {
                CRM = "123456",
                Senha = "password"
            };

            _mockRepository.Setup(repo => repo.GetByCRMAsync(It.IsAny<string>())).ReturnsAsync((Medico)null);

            // Act
            var result = await _service.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal("CRM ou Senha inválidos.", result.Message);
        }
    }
}

