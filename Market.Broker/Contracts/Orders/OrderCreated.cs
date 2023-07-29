using System.Text.Json.Serialization;

namespace Market.Broker.Contracts.Orders {
    public record OrderCreated {
        public Guid Id { get; init; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;
        [JsonPropertyName("category")]
        public string Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
