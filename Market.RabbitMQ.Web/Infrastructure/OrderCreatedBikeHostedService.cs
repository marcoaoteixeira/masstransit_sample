using System.Text.Json;
using Market.Broker.Contracts.Orders;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Market.RabbitMQ.Web.Infrastructure {
    public class OrderCreatedBikeHostedService : BackgroundService {
        private readonly ILogger<OrderCreatedBikeHostedService> _logger;
        private readonly IModel _channel;

        public OrderCreatedBikeHostedService(ILogger<OrderCreatedBikeHostedService> logger, IModel channel) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            _channel.ExchangeDeclare("order.created", "direct", durable: true, autoDelete: false);

            _channel.ExchangeDeclare("order.created.bike", "fanout", durable: true, autoDelete: false);
            _channel.ExchangeBind("order.created.bike", "order.created", "bike");

            _channel.QueueDeclare("order.created.bike", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind("order.created.bike", "order.created.bike", string.Empty);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += HandleMessage;

            _channel.BasicConsume("order.created.bike", false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(object? sender, BasicDeliverEventArgs args) {
            var consumer = sender as EventingBasicConsumer;
            var message = JsonSerializer.Deserialize<object>(args.Body.Span);

            if (message != null) {
                var json = JsonSerializer.Serialize(message);
                _logger.LogInformation("Order created: {0}", json);
            }

            consumer?.Model.BasicAck(args.DeliveryTag, multiple: false);
        }
    }
}
