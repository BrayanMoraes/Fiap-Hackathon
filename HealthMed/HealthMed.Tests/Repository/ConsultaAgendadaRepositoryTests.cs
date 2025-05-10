using HealthMed.Domain.Entities;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Tests.Repository
{
    public class ConsultaAgendadaRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddConsultaToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new ConsultaAgendadaRepository(dbContext);

            var consulta = new ConsultaAgendada
            {
                Id = Guid.NewGuid(),
                IdPaciente = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                IdAgenda = Guid.NewGuid(),
                Aprovado = true
            };

            // Act
            await repository.AddAsync(consulta);

            // Assert
            var savedConsulta = await dbContext.ConsultasAgendadas.FirstOrDefaultAsync(c => c.Id == consulta.Id);
            Assert.NotNull(savedConsulta);
            Assert.Equal(consulta.IdPaciente, savedConsulta.IdPaciente);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateConsultaInDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new ConsultaAgendadaRepository(dbContext);

            var consulta = new ConsultaAgendada
            {
                Id = Guid.NewGuid(),
                IdPaciente = Guid.NewGuid(),
                IdMedico = Guid.NewGuid(),
                IdAgenda = Guid.NewGuid(),
                Aprovado = true
            };

            await dbContext.ConsultasAgendadas.AddAsync(consulta);
            await dbContext.SaveChangesAsync();

            // Act
            consulta.Aprovado = false;
            await repository.UpdateAsync(consulta);

            // Assert
            var updatedConsulta = await dbContext.ConsultasAgendadas.FirstOrDefaultAsync(c => c.Id == consulta.Id);
            Assert.NotNull(updatedConsulta);
            Assert.False(updatedConsulta.Aprovado);
        }
    }
}
