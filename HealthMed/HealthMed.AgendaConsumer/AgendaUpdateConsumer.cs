using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Queue;
using HealthMed.Domain.Interfaces.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HealthMed.AgendaConsumer
{
    public class AgendaUpdateConsumer : IQueueConsumer
    {
        private readonly IChannel _channel;
        private readonly IAgendaRepository _agendaRepository;

        public AgendaUpdateConsumer(IConnection rabbitMqConnection, IAgendaRepository agendaRepository)
        {
            _channel = rabbitMqConnection.CreateChannelAsync().GetAwaiter().GetResult();
            _agendaRepository = agendaRepository;
        }

        public async Task StartConsuming()
        {
            await _channel.QueueDeclareAsync(queue: "agenda.update", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida na fila 'agenda.update': {message}");

                try
                {
                    var agenda = JsonSerializer.Deserialize<Agenda>(message);
                    if (agenda != null)
                    {
                        var agendaExistente = await _agendaRepository.GetByIdAsync(agenda.Id);

                        if (agendaExistente == null)
                        {
                            Console.WriteLine($"Agenda com ID {agenda.Id} não encontrada.");
                            return;
                        }

                        agendaExistente.Data = agenda.Data;
                        agendaExistente.ValorConsulta = agenda.ValorConsulta;
                        agendaExistente.Horario = agenda.Horario;

                        await _agendaRepository.UpdateAsync(agendaExistente);
                        Console.WriteLine("Agenda atualizada com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem da fila 'agenda.update': {ex.Message}");
                }
            };

            await _channel.BasicConsumeAsync(queue: "agenda.update", autoAck: true, consumer: consumer);
        }
    }
}
