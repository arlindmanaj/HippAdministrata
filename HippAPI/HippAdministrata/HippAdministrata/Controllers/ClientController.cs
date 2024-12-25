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
        public async Task<IActionResult> CreateMultipleOrders(int clientId, [FromBody] OrderDto orderDto)
        {
            try
            {
                var orders = await _orderService.CreateMultipleOrdersAsync(clientId, orderDto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
