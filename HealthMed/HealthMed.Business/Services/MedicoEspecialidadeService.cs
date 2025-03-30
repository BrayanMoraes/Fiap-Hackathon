using HealthMed.Domain.DTO;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace HealthMed.Business.Services
{
    internal class MedicoEspecialidadeService : IMedicoEspecialidadeService
    {
        private readonly IMedicoEspecialidadeRepository _repository;
        private readonly IDatabase _redisCache;

        public MedicoEspecialidadeService(IMedicoEspecialidadeRepository repository, IConnectionMultiplexer redisConnection)
        {
            _repository = repository;
            _redisCache = redisConnection.GetDatabase();
        }

        public async Task<ICollection<MedicoEspecialidadeDTO>> ObterTodasEspecialidades()
        {
            const string cacheKey = "MedicoEspecialidades";

            var cachedData = await _redisCache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<ICollection<MedicoEspecialidadeDTO>>(cachedData);
            }

            var especialidades = await _repository.ObterTodasEspecialidades();

            await _redisCache.StringSetAsync(
                cacheKey,
                JsonSerializer.Serialize(especialidades),
                TimeSpan.FromHours(1)
            );

            return especialidades;
        }
    }
}