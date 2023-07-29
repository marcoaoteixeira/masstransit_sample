using Market.Core;
using MassTransit;

namespace Market.MassTransit.Web.Infrastructure {
    public partial class ProducerService : IProducerService {
        private readonly IPublishEndpoint _endpoint;
        public ProducerService(IPublishEndpoint endpoint) {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }
        public Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
            where T : class => _endpoint.Publish(message, cancellationToken);
    }
}
