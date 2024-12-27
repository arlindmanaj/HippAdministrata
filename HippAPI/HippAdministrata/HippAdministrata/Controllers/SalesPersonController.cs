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

        [HttpPut("orders/{orderId}/update-assignment")]
        public async Task<IActionResult> UpdateOrderAssignment(int orderId, [FromBody] OrderAssignmentDto assignmentDto)
        {
            try
            {
                var order = await _orderService.UpdateOrderAssignmentAsync(orderId, assignmentDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersBySalesPersonId(int salesPersonId)
        {
          

            var orders = await _orderService.GetOrdersBySalesPersonIdAsync(salesPersonId);
            return Ok(orders);
        }


    }
}
