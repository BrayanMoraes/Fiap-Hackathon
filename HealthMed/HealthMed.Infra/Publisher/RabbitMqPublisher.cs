using RabbitMQ.Client;
using System.Text;

namespace HealthMed.Infra.Publisher
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;

        public RabbitMqPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishMessage(string exchange, string routingKey, string message)
        {
            using var channel = await _connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange,
                routingKey,
                true,
                body: body
            );
        }
    }
}
