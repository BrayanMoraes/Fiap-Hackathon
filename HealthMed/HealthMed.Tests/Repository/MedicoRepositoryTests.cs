using HealthMed.Domain.Entities;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repository;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Tests.Repository
{
    public class MedicoRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMedicoToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new MedicoRepository(dbContext);

            var medico = new Medico
            {
                Id = Guid.NewGuid(),
                CRM = "123456",
                Nome = "Dr. Teste",
                Senha = "hashedpassword"
            };

            // Act
            await repository.AddAsync(medico);

            // Assert
            var savedMedico = await dbContext.Medicos.FirstOrDefaultAsync(m => m.Id == medico.Id);
            Assert.NotNull(savedMedico);
            Assert.Equal(medico.CRM, savedMedico.CRM);
        }

        [Fact]
        public async Task GetByCRMAsync_ShouldReturnMedico_WhenCRMMatches()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new MedicoRepository(dbContext);

            var medico = new Medico
            {
                Id = Guid.NewGuid(),
                CRM = "123456",
                Nome = "Dr. Teste",
                Senha = "hashedpassword"
            };

            await dbContext.Medicos.AddAsync(medico);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByCRMAsync("123456");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(medico.CRM, result.CRM);
        }
    }
}
