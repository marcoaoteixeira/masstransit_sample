namespace Market.Core {
    public interface IProducerService {
        Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
            where T : class;
    }
}