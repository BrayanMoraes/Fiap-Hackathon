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
    public class AgendaControllerTests
    {
        private readonly Mock<IAgendaService> _mockService;
        private readonly AgendaController _controller;

        public AgendaControllerTests()
        {
            _mockService = new Mock<IAgendaService>();
            _controller = new AgendaController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenAgendaExists()
        {
            // Arrange
            var agendaId = Guid.NewGuid();
            var agendaDto = new AgendaRetornoDTO
            {
                Id = agendaId,
                IdMedico = Guid.NewGuid(),
                Horario = TimeSpan.FromHours(10),
                Data = DateTime.UtcNow,
                ValorConsulta = 150.00m
            };

            _mockService
                .Setup(service => service.GetByIdAsync(agendaId))
                .ReturnsAsync(new OperationResult<AgendaRetornoDTO>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agendaDto
                });

            // Act
            var result = await _controller.GetById(agendaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationResult<AgendaRetornoDTO>>(okResult.Value);
            Assert.Equal(agendaId, response.ResultObject.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenAgendaDoesNotExist()
        {
            // Arrange
            var agendaId = Guid.NewGuid();

            _mockService
                .Setup(service => service.GetByIdAsync(agendaId))
                .ReturnsAsync(new OperationResult<AgendaRetornoDTO>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "Agenda não encontrada.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.GetById(agendaId);

            // Assert
            var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<OperationResult<AgendaRetornoDTO>>(notFoundResult.Value);
            Assert.Equal("Agenda não encontrada.", response.Message);
        }
    }
}
