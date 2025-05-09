using HealthMed.Business.Services;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using Moq;
using RabbitMQ.Client;

namespace HealthMed.Tests.Services
{
    public class AgendaServiceTests
    {
        private readonly Mock<IAgendaRepository> _mockRepository;
        private readonly Mock<IConnection> _mockRabbitMqConnection;
        private readonly AgendaService _service;

        public AgendaServiceTests()
        {
            _mockRepository = new Mock<IAgendaRepository>();
            _mockRabbitMqConnection = new Mock<IConnection>();
            _service = new AgendaService(_mockRepository.Object, _mockRabbitMqConnection.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAgenda_WhenAgendaExists()
        {
            // Arrange
            var agenda = new Agenda
            {
                Id = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                Horario = TimeSpan.FromHours(10),
                Data = DateTime.UtcNow,
                ValorConsulta = 150.00m
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(agenda.Id)).ReturnsAsync(agenda);

            // Act
            var result = await _service.GetByIdAsync(agenda.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Success, result.Status);
            Assert.Equal(agenda.Id, result.ResultObject.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnError_WhenAgendaIdIsEmpty()
        {
            // Arrange
            var agendaDto = new GerenciarAgendaDTO
            {
                Id = Guid.Empty,
                IdMedico = Guid.NewGuid(),
                Horario = TimeSpan.FromHours(10),
                Data = DateTime.UtcNow.AddDays(1),
                ValorConsulta = 150.00m
            };

            // Act
            var result = await _service.UpdateAsync(agendaDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal("O ID da agenda não foi fornecido.", result.Message);
        }
    }
}

