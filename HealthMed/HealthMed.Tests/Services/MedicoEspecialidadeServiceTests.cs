using HealthMed.Business.Services;
using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Repository;
using Moq;
using StackExchange.Redis;

namespace HealthMed.Tests.Services
{
    public class MedicoEspecialidadeServiceTests
    {
        private readonly Mock<IMedicoEspecialidadeRepository> _mockRepository;
        private readonly MedicoEspecialidadeService _service;

        public MedicoEspecialidadeServiceTests()
        {
            _mockRepository = new Mock<IMedicoEspecialidadeRepository>();
            var mockRedisCache = new Mock<IConnectionMultiplexer>(); // Added missing dependency
            _service = new MedicoEspecialidadeService(_mockRepository.Object, mockRedisCache.Object);
        }

        [Fact]
        public async Task ObterTodasEspecialidades_ShouldReturnEspecialidades_WhenTheyExist()
        {
            // Arrange
            var especialidades = new List<MedicoEspecialidadeDTO>
            {
                new MedicoEspecialidadeDTO { IdEspecialidade = Guid.NewGuid(), DescricaoEspecialidade = "Cardiologia" },
                new MedicoEspecialidadeDTO { IdEspecialidade = Guid.NewGuid(), DescricaoEspecialidade = "Dermatologia" }
            };

            _mockRepository.Setup(repo => repo.ObterTodasEspecialidades()).ReturnsAsync(especialidades);

            var mockDatabase = new Mock<IDatabase>();
            var mockRedisCache = new Mock<IConnectionMultiplexer>();
            mockRedisCache.Setup(conn => conn.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);

            var service = new MedicoEspecialidadeService(_mockRepository.Object, mockRedisCache.Object);

            // Act
            var result = await service.ObterTodasEspecialidades();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ResultObject.Count);
            Assert.Contains(result.ResultObject, e => e.DescricaoEspecialidade == "Cardiologia");
        }

        [Fact]
        public async Task ObterTodasEspecialidades_ShouldReturnEmptyList_WhenNoEspecialidadesExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ObterTodasEspecialidades()).ReturnsAsync(new List<MedicoEspecialidadeDTO>());

            var mockDatabase = new Mock<IDatabase>();
            var mockRedisCache = new Mock<IConnectionMultiplexer>();
            mockRedisCache.Setup(conn => conn.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);

            var service = new MedicoEspecialidadeService(_mockRepository.Object, mockRedisCache.Object);

            // Act
            var result = await service.ObterTodasEspecialidades();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.ResultObject);
        }
    }
}


