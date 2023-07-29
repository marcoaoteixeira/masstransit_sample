namespace Market.RabbitMQ.Web.Models {
    public record CreateOrderInput {
        public string Description { get; init; } = null!;
        public string Category { get; init; } = null!;
    }
}
