using HippAdministrata.Models.DTOs;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public ClientController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{clientId}/orders")]
        public async Task<IActionResult> CreateOrder(int clientId, [FromBody] OrderDto orderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(clientId, orderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
