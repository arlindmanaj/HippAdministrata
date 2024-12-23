using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPersonController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public SalesPersonController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPut("orders/{orderId}/assign")]
        public async Task<IActionResult> AssignOrder(int orderId, [FromBody] OrderAssignmentDto assignmentDto)
        {
            try
            {
                var order = await _orderService.AssignOrderAsync(orderId, assignmentDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
