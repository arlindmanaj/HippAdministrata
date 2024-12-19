using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Services;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HippAdministrata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPersonController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ISalesPersonService _salesPersonService;
        private readonly IClientService _clientService;
        private readonly ILogger _logger;
        private readonly ISalesPersonClientService _salesPersonClientService;
        

        public SalesPersonController(IOrderService orderService, ISalesPersonClientService salesPersonClientService , ISalesPersonService salesPersonService, IClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
           
            _salesPersonClientService = salesPersonClientService;
            _salesPersonService = salesPersonService;
        }




        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            if (!result) return NotFound($"Order with ID {orderId} not found.");

            return Ok("Order status updated successfully.");
        }


        [HttpGet("order-requests")]
        public async Task<IActionResult> GetClientOrderRequests()
        {
            try
            {
                var requests = await _salesPersonClientService.GetAllPendingAsync();
                if (!requests.Any()) return NotFound("No client order requests found.");

                return Ok(requests);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        
        [HttpPost("process-order/{requestId}")]
        public async Task<IActionResult> ProcessOrderRequest(int requestId, [FromBody] OrderProcessRequestDto request)
        {
            try
            {
                var result = await _salesPersonClientService.ProcessOrderRequestAsync(requestId, request);
                if (!result) return BadRequest("Failed to process order request.");

                return Ok("Order processed successfully.");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
