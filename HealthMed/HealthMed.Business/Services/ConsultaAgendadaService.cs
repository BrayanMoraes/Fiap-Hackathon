using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HealthMed.Business.Services
{
    public class ConsultaAgendadaService : IConsultaAgendadaService
    {
        private readonly IConsultaAgendadaRepository _repository;
        private readonly IConnection _rabbitMqConnection;

        public ConsultaAgendadaService(IConsultaAgendadaRepository repository, IConnection rabbitMqConnection)
        {
            _repository = repository;
            _rabbitMqConnection = rabbitMqConnection;
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
                        Message = "Consulta n�o encontrada."
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

        public async Task<OperationResult<object>> AddAsync(ConsultaAgendadaDTO consultaAgendada)
        {
            try
            {
                if (consultaAgendada.Solicitante == Solicitante.Paciente && consultaAgendada.Aprovado != null)
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Paciente n�o pode aprovar consulta!"
                    };
                }

                var message = JsonSerializer.Serialize(consultaAgendada);
                await PublishMessageAsync("consulta.add", message);

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

        public async Task<OperationResult<object>> UpdateAsync(ConsultaAgendadaDTO consultaAgendada)
        {
            try
            {
                if(consultaAgendada.Solicitante == Solicitante.Paciente && consultaAgendada.Aprovado != null)
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Paciente n�o pode aprovar consulta!"
                    };
                }

                var message = JsonSerializer.Serialize(consultaAgendada);
                await PublishMessageAsync("consulta.update", message);

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
                return ServiceHelper.HandleException<IEnumerable<ConsultaAgendada>>(ex, "Erro ao buscar consultas por m�dico.");
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
                        Message = "Consulta n�o encontrada."
                    };
                }

                consulta.Cancelada = true;
                consulta.MotivoCancelamento = motivoCancelamento;

                var message = JsonSerializer.Serialize(consulta);
                await PublishMessageAsync("consulta.cancel", message);

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

        private async Task PublishMessageAsync(string queueName, string message)
        {
            using var channel = await _rabbitMqConnection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                             routingKey: queueName,
                                             body: body);
        }
    }
}
