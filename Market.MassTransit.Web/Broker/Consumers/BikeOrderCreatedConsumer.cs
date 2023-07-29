using System.Text.Json;
using Market.Broker.Contracts.Orders;
using MassTransit;
using RabbitMQ.Client;

namespace Market.MassTransit.Web.Domain.Order {
    public class BikeOrderCreatedConsumer : IConsumer<OrderCreated> {
        private readonly ILogger<BikeOrderCreatedConsumer> _logger;
        public BikeOrderCreatedConsumer(ILogger<BikeOrderCreatedConsumer> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public Task Consume(ConsumeContext<OrderCreated> context) {
            var json = JsonSerializer.Serialize(context.Message);
            _logger.LogInformation("Order created: {0}", json);
            return Task.CompletedTask;
        }
    }

    public class BikeOrderCreatedConsumerDefinition : ConsumerDefinition<BikeOrderCreatedConsumer> {
        public BikeOrderCreatedConsumerDefinition() {
            EndpointName = "order.created.bike";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<BikeOrderCreatedConsumer> consumerConfigurator) {
            endpointConfigurator.ConfigureConsumeTopology = false;
            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator configurator) {
                configurator.Bind<OrderCreated>(_ => {
                    _.RoutingKey = "bike";
                    _.ExchangeType = ExchangeType.Direct;
                });
            }
        }
    }
}
