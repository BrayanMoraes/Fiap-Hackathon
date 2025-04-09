using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using HealthMed.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<OperationResult<object>> AddAsync(ConsultaAgendada consultaAgendada)
        {
            await _repository.AddAsync(consultaAgendada);
            return new OperationResult<object>
            {
                Status = TypeReturnStatus.Success,
                Message = "Consulta agendada com sucesso."
            };
        }

        public async Task<OperationResult<object>> UpdateAsync(ConsultaAgendada consultaAgendada)
        {
            await _repository.UpdateAsync(consultaAgendada);
            return new OperationResult<object>
            {
                Status = TypeReturnStatus.Success,
                Message = "Consulta atualizada com sucesso."
            };
        }

        public async Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByMedicoIdAsync(Guid medicoId)
        {
            var consultas = await _repository.GetByMedicoIdAsync(medicoId);
            return new OperationResult<IEnumerable<ConsultaAgendada>>
            {
                Status = TypeReturnStatus.Success,
                ResultObject = consultas
            };
        }

        public async Task<OperationResult<IEnumerable<ConsultaAgendada>>> GetByPacienteIdAsync(Guid pacienteId)
        {
            var consultas = await _repository.GetByPacienteIdAsync(pacienteId);
            return new OperationResult<IEnumerable<ConsultaAgendada>>
            {
                Status = TypeReturnStatus.Success,
                ResultObject = consultas
            };
        }

        public async Task<OperationResult<object>> CancelarConsultaAsync(Guid id, string motivoCancelamento)
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
    }
}
