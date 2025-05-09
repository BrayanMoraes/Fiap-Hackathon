using HealthMed.Business.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using Moq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Xunit;

namespace HealthMed.Tests.Services
{
    public class ConsultaAgendadaServiceTests
    {
        private readonly Mock<IConsultaAgendadaRepository> _mockRepository;
        private readonly Mock<IConnection> _mockRabbitMqConnection;
        private readonly ConsultaAgendadaService _service;

        public ConsultaAgendadaServiceTests()
        {
            _mockRepository = new Mock<IConsultaAgendadaRepository>();
            _mockRabbitMqConnection = new Mock<IConnection>();
            _service = new ConsultaAgendadaService(_mockRepository.Object, _mockRabbitMqConnection.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnConsulta_WhenConsultaExists()
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

            _mockRepository.Setup(repo => repo.GetByIdAsync(consulta.Id)).ReturnsAsync(consulta);

            // Act
            var result = await _service.GetByIdAsync(consulta.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Success, result.Status);
            Assert.Equal(consulta.Id, result.ResultObject.Id);
        } 
    }
}

