using Market.Broker.Contracts.Orders;
using Market.Core;
using Market.MassTransit.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.MassTransit.Web.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase {
        private readonly IProducerService _producerService;
        public OrderController(IProducerService producerService) {
            _producerService = producerService ?? throw new ArgumentNullException(nameof(producerService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrderInput input, CancellationToken cancellationToken = default) {
            var orderCreated = new OrderCreated {
                Id = Guid.NewGuid(),
                Description = input.Description,
                Category = input.Category,
                CreatedAt = DateTime.UtcNow,
            };

            await _producerService.ProduceAsync("", orderCreated, cancellationToken);

            return Ok();
        }
    }
}
