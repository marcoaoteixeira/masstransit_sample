namespace Market.MassTransit.Web.Models {
    public record CreateOrderOutput {
        public Guid Id { get; set; }
        public string Description { get; init; } = null!;
        public string Category { get; init; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
