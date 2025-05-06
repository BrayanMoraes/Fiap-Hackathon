using HealthMed.API.Controllers;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HealthMed.Tests.Controllers
{
    public class ConsultaAgendadaControllerTests
    {
        private readonly Mock<IConsultaAgendadaService> _mockService;
        private readonly ConsultaAgendadaController _controller;

        public ConsultaAgendadaControllerTests()
        {
            _mockService = new Mock<IConsultaAgendadaService>();
            _controller = new ConsultaAgendadaController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenConsultaExists()
        {
            // Arrange
            var consultaId = Guid.NewGuid();
            var consulta = new ConsultaAgendada
            {
                Id = consultaId,
                IdPaciente = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                IdAgenda = Guid.NewGuid(),
                Aprovado = true
            };

            _mockService
                .Setup(service => service.GetByIdAsync(consultaId))
                .ReturnsAsync(new OperationResult<ConsultaAgendada>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = consulta
                });

            // Act
            var result = await _controller.GetById(consultaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ConsultaAgendada>(okResult.Value);
            Assert.Equal(consultaId, response.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenConsultaDoesNotExist()
        {
            // Arrange
            var consultaId = Guid.NewGuid();

            _mockService
                .Setup(service => service.GetByIdAsync(consultaId))
                .ReturnsAsync(new OperationResult<ConsultaAgendada>
                {
                    Status = TypeReturnStatus.Error,
                    Message = "Consulta não encontrada.",
                    ResultObject = null
                });

            // Act
            var result = await _controller.GetById(consultaId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Consulta não encontrada.", response);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenConsultaIsAddedSuccessfully()
        {
            // Arrange
            var consulta = new ConsultaAgendada
            {
                Id = Guid.NewGuid(),
                IdPaciente = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                IdAgenda = Guid.NewGuid(),
                Aprovado = true
            };

            _mockService
                .Setup(service => service.AddAsync(consulta))
                .ReturnsAsync(new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Consulta agendada com sucesso."
                });

            // Act
            var result = await _controller.Add(consulta);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Consulta agendada com sucesso.", response);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var consultaId = Guid.NewGuid();
            var consulta = new ConsultaAgendada
            {
                Id = Guid.NewGuid(), // Different ID
                IdPaciente = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                IdAgenda = Guid.NewGuid(),
                Aprovado = true
            };

            // Act
            var result = await _controller.Update(consultaId, consulta);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CancelarConsulta_ShouldReturnOk_WhenConsultaIsCancelledSuccessfully()
        {
            // Arrange
            var consultaId = Guid.NewGuid();
            var motivoCancelamento = "Paciente indisponível";

            _mockService
                .Setup(service => service.CancelarConsultaAsync(consultaId, motivoCancelamento))
                .ReturnsAsync(new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Consulta cancelada com sucesso."
                });

            // Act
            var result = await _controller.CancelarConsulta(consultaId, motivoCancelamento);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Consulta cancelada com sucesso.", response);
        }
    }
}
