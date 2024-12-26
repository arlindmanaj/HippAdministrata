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
        private readonly IUserService _userService;

        public ClientController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
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
        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _userService.GetAllClientsAsync();
            return Ok(clients);
        }
    }

}
