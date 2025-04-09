using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Shared;
using HealthMed.Domain.Enum;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<OperationResult<Agenda>> GetByIdAsync(Guid id)
        {
            try
            {
                var agenda = await _repository.GetByIdAsync(id);
                if (agenda == null)
                {
                    return new OperationResult<Agenda>
                    {
                        Status = TypeReturnStatus.Error,
                        Message = "Agenda não encontrada."
                    };
                }

                return new OperationResult<Agenda>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agenda
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<Agenda>(ex, "Erro ao buscar agenda.");
            }
        }

        public async Task<OperationResult<object>> AddAsync(Agenda agenda)
        {
            try
            {
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

        public async Task<OperationResult<object>> UpdateAsync(Agenda agenda)
        {
            try
            {
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

        public async Task<OperationResult<IEnumerable<Agenda>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var agendas = await _repository.GetByMedicoIdAsync(medicoId);
                return new OperationResult<IEnumerable<Agenda>>
                {
                    Status = TypeReturnStatus.Success,
                    ResultObject = agendas
                };
            }
            catch (Exception ex)
            {
                return ServiceHelper.HandleException<IEnumerable<Agenda>>(ex, "Erro ao buscar agendas por médico.");
            }
        }

        private async Task PublishMessageAsync(string routingKey, string message)
        {
            using var channel = await _rabbitMqConnection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: "agenda_exchange", type: ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "agenda_exchange",
                                 routingKey: routingKey,
                                 body: body);
        }
    }
}
