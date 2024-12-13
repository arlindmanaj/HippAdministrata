using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (await _orderService.CreateOrderAsync(order)) return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if (await _orderService.UpdateOrderAsync(order)) return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (await _orderService.DeleteOrderAsync(id)) return NoContent();
            return NotFound();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            if (await _orderService.UpdateOrderStatusAsync(id, status)) return NoContent();
            return BadRequest();
        }
    }
}
