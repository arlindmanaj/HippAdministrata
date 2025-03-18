using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.Requests;
using HippAdministrata.Services;
using HippAdministrata.Services.Implementation;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HippAdministrata.Models.Dtos;

namespace HippAdministrata.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRequestService _orderRequestService;

        public OrderController(IOrderService orderService, IOrderRequestService orderRequestService)
        {
            _orderService = orderService;
            _orderRequestService = orderRequestService;


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var orders = await _orderService.GetByClientIdAsync(clientId);
            return Ok(orders);
        }

        //[Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result) return NotFound("Order not found.");
            return Ok("Order deleted successfully.");
        }

        // Notification Request for Order

        // Get all pending requests (update/delete)
        ////[Authorize(Roles = "Manager")]
        //[HttpGet("pending-requests")]
        //public async Task<IActionResult> GetPendingRequests()
        //{
        //    var requests = await _orderService.GetPendingRequestsAsync();
        //    return Ok(requests);
        //}

        //// Approve an order request (update/delete)
        ////[Authorize(Roles = "Manager")]
        //[HttpPost("approve-request/{requestId}")]
        //public async Task<IActionResult> ApproveRequest(int requestId)
        //{
        //    var result = await _orderService.ApproveRequestAsync(requestId);
        //    if (!result)
        //        return NotFound("Request not found or already processed.");
        //    return Ok("Request approved successfully.");
        //}

        //// Reject an order request (update/delete)  
        ////[Authorize(Roles = "Manager")]
        //[HttpPost("reject-request/{requestId}")]
        //public async Task<IActionResult> RejectRequest(int requestId)
        //{
        //    var result = await _orderService.RejectRequestAsync(requestId);
        //    if (!result)
        //        return NotFound("Request not found or already processed.");
        //    return Ok("Request rejected.");
        //}


        //[HttpPost("request")]
        //public async Task<IActionResult> CreateOrderRequest([FromBody] CreateOrderRequestDto requestDto)
        //{
        //    var request = await _orderRequestService.CreateOrderRequestAsync(requestDto.OrderId, requestDto.ClientId, requestDto.RequestType, requestDto.Reason);
        //    return Ok(request);
        //}
        [HttpPost("request")]
        public async Task<IActionResult> CreateOrderRequest([FromBody] CreateOrderRequestDto requestDto)
        {
            // Validate request type
            if (requestDto.RequestType != "Update" && requestDto.RequestType != "Delete")
                return BadRequest("Request type must be either 'Update' or 'Delete'.");

            // Call the service method, passing all necessary properties
            var request = await _orderRequestService.CreateOrderRequestAsync(
                requestDto.OrderId,
                requestDto.ClientId,
                requestDto.RequestType,
                requestDto.Reason,
                requestDto.NewDeliveryDestination,
                requestDto.NewQuantity,
                requestDto.NewProductId);

            return Ok(request);
        }

        // OrderController.cs

        [HttpPut("update/{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(orderId, updateOrderDto);
                return Ok(updatedOrder); // Return the updated order
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle case where order is not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Generic error handling
            }
        }


        [HttpGet("pending-requests")]
            public async Task<IActionResult> GetPendingRequests()
            {
                var requests = await _orderRequestService.GetPendingRequestsAsync();
                return Ok(requests);
            }

            [HttpPost("approve-request/{requestId}")]
            public async Task<IActionResult> ApproveRequest(int requestId)
            {
                var success = await _orderRequestService.ApproveRequestAsync(requestId);
            return success ? Ok(new { message = "Request Approved" })
            : BadRequest(new { message = "Failed to reject request." });
            }

            [HttpPost("reject-request/{requestId}")]
            public async Task<IActionResult> RejectRequest(int requestId, [FromBody] RejectRequestDto rejectDto)
            {
                var success = await _orderRequestService.RejectRequestAsync(requestId, rejectDto.Reason);
            return success ? Ok(new { message = "Request rejected" })
           : BadRequest(new { message = "Failed to reject request." });

        }
    }


    }
    

