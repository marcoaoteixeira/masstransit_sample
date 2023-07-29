using Market.Core;
using RabbitMQ.Client;

namespace Market.RabbitMQ.Web.Infrastructure {
    public static class ServiceCollectionExtension {
        public static IServiceCollection RegisterProducer(this IServiceCollection self)
            => self.AddScoped<IProducerService, ProducerService>();

        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection self) {
            var factory = new ConnectionFactory {
                HostName = "localhost",
                Port = 55250,
                UserName = "guest",
                Password = "guest"
            };

            self.AddSingleton((provider) => factory.CreateConnection());
            self.AddSingleton((provider) => provider.GetService<IConnection>().CreateModel());

            return self;
        }
    }
}
