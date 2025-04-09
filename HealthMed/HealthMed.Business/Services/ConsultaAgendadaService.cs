using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;

namespace HealthMed.Business.Services
{
    public class ConsultaAgendadaService : IConsultaAgendadaService
    {
        private readonly IConsultaAgendadaRepository _repository;

        public ConsultaAgendadaService(IConsultaAgendadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<ConsultaAgendada>> GetByIdAsync(Guid id)
        {
            try
            {
                var consulta = await _repository.GetByIdAsync(id);
                if (consulta == null)
                {
                    return new OperationResult<ConsultaAgendada>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Consulta não encontrada."
                    };
                }

                return new OperationResult<ConsultaAgendada>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = consulta
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<ConsultaAgendada>(ex, "Erro ao buscar consulta.");
            }
        }

        public async Task<OperationResult<object>> AddAsync(ConsultaAgendada consultaAgendada)
        {
            try
            {
                await _repository.AddAsync(consultaAgendada);
                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Consulta agendada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao agendar consulta.");
            }
        }

        public async Task<OperationResult<object>> UpdateAsync(ConsultaAgendada consultaAgendada)
        {
            try
            {
                await _repository.UpdateAsync(consultaAgendada);
                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Consulta atualizada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao atualizar consulta.");
            }
        }

        public async Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var consultas = await _repository.GetByMedicoIdAsync(medicoId);
                return new OperationResult<IEnumerable<ConsultaAgendada>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = consultas
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<ConsultaAgendada>>(ex, "Erro ao buscar consultas por médico.");
            }
        }

        public async Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByPacienteIdAsync(Guid pacienteId)
        {
            try
            {
                var consultas = await _repository.GetByPacienteIdAsync(pacienteId);
                return new OperationResult<IEnumerable<ConsultaAgendada>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = consultas
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<ConsultaAgendada>>(ex, "Erro ao buscar consultas por paciente.");
            }
        }

        public async Task<OperationResult<object>> CancelarConsultaAsync(Guid id, string motivoCancelamento)
        {
            try
            {
                var consulta = await _repository.GetByIdAsync(id);
                if (consulta == null)
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Consulta não encontrada."
                    };
                }

                consulta.Cancelada = true;
                consulta.MotivoCancelamento = motivoCancelamento;
                await _repository.UpdateAsync(consulta);

                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Consulta cancelada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao cancelar consulta.");
            }
        }
    }
}
