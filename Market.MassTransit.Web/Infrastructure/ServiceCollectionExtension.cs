using Market.Broker.Contracts.Orders;
using Market.Core;
using MassTransit;

namespace Market.MassTransit.Web.Infrastructure {
    public static class ServiceCollectionExtension {
        public static IServiceCollection RegisterProducer(this IServiceCollection self)
            => self.AddScoped<IProducerService, ProducerService>();

        public static IServiceCollection RegisterMassTransit(this IServiceCollection self)
            => self.AddMassTransit(setup => {
                setup.SetKebabCaseEndpointNameFormatter();
                //setup.AddConsumers(typeof(Program).Assembly);

                setup.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host("localhost", 55250, "/", host => {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ConfigureMessageTopology();
                    cfg.ConfigureEndpoints(ctx);
                });
            });
    }
}
