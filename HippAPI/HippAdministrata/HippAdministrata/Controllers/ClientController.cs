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
        private readonly IClientService _clientService;

        public ClientController(IOrderService orderService, IUserService userService, IClientService clientService)
        {
            _orderService = orderService;
            _userService = userService;
            _clientService = clientService;
        }


        [HttpPost("{clientId}/orders")]
        public async Task<IActionResult> CreateMultipleOrders(int clientId, [FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var orders = await _orderService.CreateMultipleOrdersAsync(clientId, createOrderDto);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetClientIdByUserId(int userId)
        {
            var clientId = await _clientService.GetClientIdByUserIdAsync(userId);
            if (clientId == null) return NotFound("Client not found");

            return Ok(clientId);
        }

    }

}
