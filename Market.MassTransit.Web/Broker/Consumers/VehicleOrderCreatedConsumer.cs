using System.Text.Json;
using Market.Broker.Contracts.Orders;
using MassTransit;
using RabbitMQ.Client;

namespace Market.MassTransit.Web.Domain.Order {
    public class VehicleOrderCreatedConsumer : IConsumer<OrderCreated> {
        private readonly ILogger<VehicleOrderCreatedConsumer> _logger;
        public VehicleOrderCreatedConsumer(ILogger<VehicleOrderCreatedConsumer> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public Task Consume(ConsumeContext<OrderCreated> context) {
            var json = JsonSerializer.Serialize(context.Message);
            _logger.LogInformation("Order created: {0}", json);
            return Task.CompletedTask;
        }
    }

    public class VehicleOrderCreatedConsumerDefinition : ConsumerDefinition<VehicleOrderCreatedConsumer> {
        public VehicleOrderCreatedConsumerDefinition() {
            EndpointName = "order.created.vehicle";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<VehicleOrderCreatedConsumer> consumerConfigurator) {
            endpointConfigurator.ConfigureConsumeTopology = false;
            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator configurator) {
                configurator.Bind<OrderCreated>(_ => {
                    _.RoutingKey = "vehicle";
                    _.ExchangeType = ExchangeType.Direct;
                });
            }
        }
    }
}
