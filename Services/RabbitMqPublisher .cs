using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Repository.RabbitMQ;

namespace TraineeManagement.api.Services
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly ConnectionFactory _factory;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);

        public RabbitMqPublisher(IConfiguration configuration)
        {
            string? rabbitMqUri = configuration["RabbitMqSettings:Uri"];

            if (rabbitMqUri == null)
            {
                throw new RabbitMqException("Rabbit Mq is not Configured");
            }

            _factory = new ConnectionFactory { Uri = new Uri(rabbitMqUri) };
        }

        private async Task InitializeChannelAsync()
        {
            if (_channel is { IsOpen: true }) return;

            await _connectionLock.WaitAsync();
            try
            {
                if (_channel is { IsOpen: true }) return;

                
                _connection ??= await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                
                await _channel.ExchangeDeclareAsync(
                    exchange: "submission-exchange",
                    type: ExchangeType.Direct,
                    durable: true
                );
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task PublishAsync<T>(string exchange, string routingKey, T message, string correlationId)
        {
            await InitializeChannelAsync();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true, // Message survives a broker crash
                CorrelationId = correlationId // Tracks execution across hops natively,
            };

            await _channel!.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
            }
        }
    }
}
