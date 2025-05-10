using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Queue;
using HealthMed.Domain.Interfaces.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HealthMed.AgendaConsumer
{
    public class AgendaAddConsumer : IQueueConsumer
    {
        private readonly IChannel _channel;
        private readonly IAgendaRepository _agendaRepository;

        public AgendaAddConsumer(IConnection rabbitMqConnection, IAgendaRepository agendaRepository)
        {
            _channel = rabbitMqConnection.CreateChannelAsync().GetAwaiter().GetResult();
            _agendaRepository = agendaRepository;
        }

        public async Task StartConsuming()
        {
            await _channel.QueueDeclareAsync(queue: "agenda.add", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida na fila 'agenda.add': {message}");

                try
                {
                    var agenda = JsonSerializer.Deserialize<Agenda>(message);
                    if (agenda != null)
                    {
                        await _agendaRepository.AddAsync(agenda);
                        Console.WriteLine("Agenda adicionada com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem da fila 'agenda.add': {ex.Message}");
                }
            };

            await _channel.BasicConsumeAsync(queue: "agenda.add", autoAck: true, consumer: consumer);
        }
    }
}
