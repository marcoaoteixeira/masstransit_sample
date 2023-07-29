namespace Market.Broker.Contracts.Orders {
    public record OrderAccepted {
        public string OrderId { get; set; } = null!;
        public DateTime AcceptedAt { get; set; }
    }
}
