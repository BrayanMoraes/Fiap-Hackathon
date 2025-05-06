using HealthMed.Domain.Entities;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Tests.Repository
{
    public class AgendaRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAgendaToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new AgendaRepository(dbContext);

            var agenda = new Agenda
            {
                Id = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                Horario = TimeSpan.FromHours(10),
                Data = DateTime.UtcNow,
                ValorConsulta = 150.00m
            };

            // Act
            await repository.AddAsync(agenda);

            // Assert
            var savedAgenda = await dbContext.Agendas.FirstOrDefaultAsync(a => a.Id == agenda.Id);
            Assert.NotNull(savedAgenda);
            Assert.Equal(agenda.IdMedico, savedAgenda.IdMedico);
            Assert.Equal(agenda.ValorConsulta, savedAgenda.ValorConsulta);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAgendaInDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new AgendaRepository(dbContext);

            var agenda = new Agenda
            {
                Id = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                Horario = TimeSpan.FromHours(10),
                Data = DateTime.UtcNow,
                ValorConsulta = 150.00m
            };

            await dbContext.Agendas.AddAsync(agenda);
            await dbContext.SaveChangesAsync();

            // Act
            agenda.ValorConsulta = 200.00m;
            await repository.UpdateAsync(agenda);

            // Assert
            var updatedAgenda = await dbContext.Agendas.FirstOrDefaultAsync(a => a.Id == agenda.Id);
            Assert.NotNull(updatedAgenda);
            Assert.Equal(200.00m, updatedAgenda.ValorConsulta);
        }

        [Fact]
        public async Task GetByMedicoIdAsync_ShouldReturnAgendasForMedico()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new AgendaRepository(dbContext);

            var medicoId = Guid.NewGuid();
            var agenda1 = new Agenda { Id = Guid.NewGuid(), IdMedico = medicoId, Data = DateTime.UtcNow, ValorConsulta = 150.00m };
            var agenda2 = new Agenda { Id = Guid.NewGuid(), IdMedico = medicoId, Data = DateTime.UtcNow.AddDays(1), ValorConsulta = 200.00m };

            await dbContext.Agendas.AddRangeAsync(agenda1, agenda2);
            await dbContext.SaveChangesAsync();

            // Act
            var agendas = await repository.GetByMedicoIdAsync(medicoId);

            // Assert
            Assert.NotNull(agendas);
            Assert.Equal(2, agendas.Count());
        }
    }
}
