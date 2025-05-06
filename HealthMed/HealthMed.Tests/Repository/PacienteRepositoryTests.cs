using HealthMed.Domain.Entities;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repository;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Tests.Repository
{
    public class PacienteRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPacienteToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new PacienteRepository(dbContext);

            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                CPF = "12345678900",
                Email = "test@example.com",
                Senha = "hashedpassword"
            };

            // Act
            await repository.AddAsync(paciente);

            // Assert
            var savedPaciente = await dbContext.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
            Assert.NotNull(savedPaciente);
            Assert.Equal(paciente.CPF, savedPaciente.CPF);
            Assert.Equal(paciente.Email, savedPaciente.Email);
        }

        [Fact]
        public async Task GetByCpfOrEmailAsync_ShouldReturnPaciente_WhenCpfMatches()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new PacienteRepository(dbContext);

            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                CPF = "12345678900",
                Email = "test@example.com",
                Senha = "hashedpassword"
            };

            await dbContext.Pacientes.AddAsync(paciente);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByCpfOrEmailAsync("12345678900", "nonexistent@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paciente.CPF, result.CPF);
        }

        [Fact]
        public async Task GetByCpfOrEmailAsync_ShouldReturnPaciente_WhenEmailMatches()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new PacienteRepository(dbContext);

            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                CPF = "12345678900",
                Email = "test@example.com",
                Senha = "hashedpassword"
            };

            await dbContext.Pacientes.AddAsync(paciente);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByCpfOrEmailAsync("nonexistentcpf", "test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paciente.Email, result.Email);
        }

        [Fact]
        public async Task GetByCpfOrEmailAsync_ShouldReturnNull_WhenNoMatchFound()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new PacienteRepository(dbContext);

            // Act
            var result = await repository.GetByCpfOrEmailAsync("nonexistentcpf", "nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }
    }
}
