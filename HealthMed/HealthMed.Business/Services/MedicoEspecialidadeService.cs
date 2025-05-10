using HealthMed.Domain.DTO;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using StackExchange.Redis;
using System.Text.Json;

namespace HealthMed.Business.Services
{
    public class MedicoEspecialidadeService : IMedicoEspecialidadeService
    {
        private readonly IMedicoEspecialidadeRepository _repository;
        private readonly IDatabase _redisCache;

        public MedicoEspecialidadeService(IMedicoEspecialidadeRepository repository, IConnectionMultiplexer redisConnection)
        {
            _repository = repository;
            _redisCache = redisConnection.GetDatabase();
        }

        public async Task<OperationResult<ICollection<MedicoEspecialidadeDTO>>> ObterTodasEspecialidades()
        {
            try
            {
                const string cacheKey = "MedicoEspecialidades";

                var cachedData = await _redisCache.StringGetAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var especialidades = JsonSerializer.Deserialize<ICollection<MedicoEspecialidadeDTO>>(cachedData);
                    return new OperationResult<ICollection<MedicoEspecialidadeDTO>>
                    {
                        Status = TypeReturnStatus.Success,
                        ResultObject = especialidades
                    };
                }

                var especialidadesDb = await _repository.ObterTodasEspecialidades();

                await _redisCache.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(especialidadesDb),
                    TimeSpan.FromHours(1)
                );

                return new OperationResult<ICollection<MedicoEspecialidadeDTO>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = especialidadesDb
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<ICollection<MedicoEspecialidadeDTO>>(ex, "Erro ao obter especialidades.");
            }
        }
    }
}
