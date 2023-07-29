using System.Text.Json;
using Market.Broker.Contracts.Orders;
using Market.Core;
using RabbitMQ.Client;

namespace Market.RabbitMQ.Web.Infrastructure {
    public class ProducerService : IProducerService {
        private readonly IModel _channel;
        public ProducerService(IModel channel) {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));

            _channel.ExchangeDeclare("order.created", "direct", durable: true, autoDelete: false);

            _channel.ExchangeDeclare("order.created.bike", "fanout", durable: true, autoDelete: false);
            _channel.ExchangeBind("order.created.bike", "order.created", "bike");

            _channel.QueueDeclare("order.created.bike", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind("order.created.bike", "order.created.bike", string.Empty);
        }
        public Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class {
            var orderCreated = message as OrderCreated;
            if (orderCreated == null) {
                return Task.CompletedTask;
            }

            var body = JsonSerializer.SerializeToUtf8Bytes(orderCreated);

            _channel.BasicPublish("order.created", orderCreated.Category, body: new ReadOnlyMemory<byte>(body));

            return Task.CompletedTask;
        }
    }
}
