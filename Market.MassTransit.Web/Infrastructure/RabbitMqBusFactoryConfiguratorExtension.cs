using Market.Broker.Contracts.Orders;
using MassTransit;
using RabbitMQ.Client;

namespace Market.MassTransit.Web.Infrastructure {
    public static class RabbitMqBusFactoryConfiguratorExtension {
        public static void ConfigureMessageTopology(this IRabbitMqBusFactoryConfigurator self) {
            self.Message<OrderCreated>(_ => _.SetEntityName("order.created"));
            self.Send<OrderCreated>(_ => {
                _.UseCorrelationId(_ => _.Id);
                _.UseRoutingKeyFormatter(_ => _.Message.Category);
            });
            self.Publish<OrderCreated>(_ => {
                _.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}
