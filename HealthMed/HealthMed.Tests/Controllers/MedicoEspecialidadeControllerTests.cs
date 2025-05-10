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
    public class MedicoEspecialidadeControllerTests
    {
        private readonly Mock<IMedicoEspecialidadeService> _mockService;
        private readonly MedicoEspecialidadeController _controller;

        public MedicoEspecialidadeControllerTests()
        {
            _mockService = new Mock<IMedicoEspecialidadeService>();
            _controller = new MedicoEspecialidadeController(_mockService.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenEspecialidadesExist()
        {
            // Arrange
            var especialidades = new List<MedicoEspecialidadeDTO>
            {
                new MedicoEspecialidadeDTO { IdEspecialidade = Guid.NewGuid(), DescricaoEspecialidade = "Cardiologia" },
                new MedicoEspecialidadeDTO { IdEspecialidade = Guid.NewGuid(), DescricaoEspecialidade = "Dermatologia" }
            };

            _mockService
                .Setup(service => service.ObterTodasEspecialidades())
                .ReturnsAsync(new OperationResult<ICollection<MedicoEspecialidadeDTO>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = especialidades
                });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<ICollection<MedicoEspecialidadeDTO>>>(okResult.Value);
            Assert.Equal(TypeReturnStatus.Success, response.Status);
            Assert.NotNull(response.ResultObject);
            Assert.Equal(2, response.ResultObject.Count);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenNoEspecialidadesExist()
        {
            // Arrange
            _mockService
                .Setup(service => service.ObterTodasEspecialidades())
                .ReturnsAsync(new OperationResult<ICollection<MedicoEspecialidadeDTO>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = new List<MedicoEspecialidadeDTO>()
                });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<ICollection<MedicoEspecialidadeDTO>>>(okResult.Value);
            Assert.Equal(TypeReturnStatus.Success, response.Status);
            Assert.NotNull(response.ResultObject);
            Assert.Empty(response.ResultObject);
        }

        [Fact]
        public async Task Get_ShouldReturnBadRequest_WhenServiceReturnsError()
        {
            // Arrange
            _mockService
                .Setup(service => service.ObterTodasEspecialidades())
                .ReturnsAsync(new OperationResult<ICollection<MedicoEspecialidadeDTO>>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "Erro ao buscar especialidades.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequestResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<ICollection<MedicoEspecialidadeDTO>>>(badRequestResult.Value);
            Assert.Equal(TypeReturnStatus.Error, response.Status);
            Assert.Equal("Erro ao buscar especialidades.", response.Message);
        }
    }
}

