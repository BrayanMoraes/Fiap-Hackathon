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
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _repository;
        private readonly IConnection _rabbitMqConnection;

        public AgendaService(IAgendaRepository repository, IConnection rabbitMqConnection)
        {
            _repository = repository;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task<OperationResult<AgendaRetornoDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var agenda = await _repository.GetByIdAsync(id);
                if (agenda == null)
                {
                    return new OperationResult<AgendaRetornoDTO>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Agenda não encontrada."
                    };
                }

                var agendaRetornoDto = new AgendaRetornoDTO
                {
                    Id = agenda.Id,
                    IdMedico = agenda.IdMedico,
                    Horario = agenda.Horario,
                    Data = agenda.Data,
                    ValorConsulta = agenda.ValorConsulta
                };

                return new OperationResult<AgendaRetornoDTO>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agendaRetornoDto
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<AgendaRetornoDTO>(ex, "Erro ao buscar agenda.");
            }
        }

        public async Task<OperationResult<object>> AddAsync(GerenciarAgendaDTO agendaDto)
        {
            try
            {
                if (!agendaDto.IsAgendaValida())
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Dados da agenda inválidos."
                    };
                }

                var agenda = new Agenda
                {
                    Id = Guid.NewGuid(),
                    IdMedico = agendaDto.IdMedico,
                    Horario = agendaDto.Horario,
                    Data = agendaDto.Data,
                    ValorConsulta = agendaDto.ValorConsulta
                };

                var message = JsonSerializer.Serialize(agenda);
                await PublishMessageAsync("agenda.add", message);

                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Agenda adicionada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao adicionar agenda.");
            }
        }

        public async Task<OperationResult<object>> UpdateAsync(GerenciarAgendaDTO agendaDto)
        {
            try
            {
                if (agendaDto.Id == Guid.Empty)
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "O ID da agenda não foi fornecido."
                    };
                }

                if (!agendaDto.IsAgendaValida())
                {
                    return new OperationResult<object>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Dados da agenda inválidos."
                    };
                }

                var agenda = new Agenda
                {
                    Id = agendaDto.Id,
                    IdMedico = agendaDto.IdMedico,
                    Horario = agendaDto.Horario,
                    Data = agendaDto.Data,
                    ValorConsulta = agendaDto.ValorConsulta
                };

                var message = JsonSerializer.Serialize(agenda);
                await PublishMessageAsync("agenda.update", message);

                return new OperationResult<object>
                {
                    Status = TypeReturnStatus.Success,
                    Message = "Agenda atualizada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<object>(ex, "Erro ao atualizar agenda.");
            }
        }

        public async Task<OperationResult<IEnumerable<AgendaRetornoDTO>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var agendas = await _repository.GetByMedicoIdAsync(medicoId);
                var agendaRetornoDtos = agendas.Select(agenda => new AgendaRetornoDTO
                {
                    Id = agenda.Id,
                    IdMedico = agenda.IdMedico,
                    Horario = agenda.Horario,
                    Data = agenda.Data,
                    ValorConsulta = agenda.ValorConsulta
                });

                return new OperationResult<IEnumerable<AgendaRetornoDTO>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agendaRetornoDtos
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<AgendaRetornoDTO>>(ex, "Erro ao buscar agendas por médico.");
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
