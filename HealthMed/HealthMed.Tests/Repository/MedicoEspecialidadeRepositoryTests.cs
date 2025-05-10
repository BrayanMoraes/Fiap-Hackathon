using HealthMed.Domain.Entities;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Tests.Repository
{
    public class MedicoEspecialidadeRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Gera um banco de dados em memória único para cada teste
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEspecialidadeToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new MedicoEspecialidadeRepository(dbContext);

            var especialidade = new MedicoEspecialidade
            {
                Id = Guid.NewGuid(),
                Descricao = "Cardiologia"
            };

            // Act
            await repository.AddAsync(especialidade);

            // Assert
            var savedEspecialidade = await dbContext.MedicoEspecialidades.FirstOrDefaultAsync(e => e.Id == especialidade.Id);
            Assert.NotNull(savedEspecialidade);
            Assert.Equal(especialidade.Descricao, savedEspecialidade.Descricao);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEspecialidades()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var repository = new MedicoEspecialidadeRepository(dbContext);

            var especialidade1 = new MedicoEspecialidade { Id = Guid.NewGuid(), Descricao = "Cardiologia" };
            var especialidade2 = new MedicoEspecialidade { Id = Guid.NewGuid(), Descricao = "Dermatologia" };

            await dbContext.MedicoEspecialidades.AddRangeAsync(especialidade1, especialidade2);
            await dbContext.SaveChangesAsync();

            // Act
            var especialidades = await repository.ObterTodasEspecialidades();

            // Assert
            Assert.NotNull(especialidades);
            Assert.Equal(2, especialidades.Count());
        }
    }
}
