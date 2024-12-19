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
        

        public SalesPersonController(ILogger<SalesPersonController> logger,IOrderService orderService, ISalesPersonClientService salesPersonClientService , ISalesPersonService salesPersonService, IClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
            _logger = logger; 
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
                _logger.LogInformation("Received ProcessOrderRequest for requestId: {RequestId}", requestId);

                var result = await _salesPersonClientService.ProcessOrderRequestAsync(requestId, request);
                if (!result)
                {
                    _logger.LogWarning("Failed to process order request for requestId: {RequestId}", requestId);
                    return BadRequest("Failed to process order request.");
                }

                _logger.LogInformation("Order processed successfully for requestId: {RequestId}", requestId);
                return Ok("Order processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the order request for requestId: {RequestId}", requestId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
