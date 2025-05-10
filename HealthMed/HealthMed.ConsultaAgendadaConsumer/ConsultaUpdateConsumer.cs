using HealthMed.Domain.DTO;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Queue;
using HealthMed.Domain.Interfaces.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HealthMed.ConsultaAgendadaConsumer
{
    public class ConsultaUpdateConsumer : IQueueConsumer
    {
        private readonly IChannel _channel;
        private readonly IConsultaAgendadaRepository _consultaAgendadaRepository;

        public ConsultaUpdateConsumer(IConnection rabbitMqConnection, IConsultaAgendadaRepository consultaAgendadaRepository)
        {
            _channel = rabbitMqConnection.CreateChannelAsync().GetAwaiter().GetResult();
            _consultaAgendadaRepository = consultaAgendadaRepository;
        }

        public async Task StartConsuming()
        {
            await _channel.QueueDeclareAsync(queue: "consulta.update", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida na fila 'consulta.update': {message}");

                try
                {
                    var consulta = JsonSerializer.Deserialize<ConsultaAgendadaDTO>(message);
                    if (consulta != null)
                    {
                        var consultaExistente = await _consultaAgendadaRepository.GetByIdAsync(consulta.Id);

                        if (consultaExistente == null)
                        {
                            Console.WriteLine($"Consulta com ID {consulta.Id} não encontrada.");
                            return;
                        }

                        consultaExistente.Aprovado = consulta.Aprovado;
                        consultaExistente.Cancelada = consulta.Cancelada;

                        await _consultaAgendadaRepository.UpdateAsync(consultaExistente);
                        Console.WriteLine("Consulta atualizada com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem da fila 'consulta.update': {ex.Message}");
                }
            };

            await _channel.BasicConsumeAsync(queue: "consulta.update", autoAck: true, consumer: consumer);
        }
    }
}
