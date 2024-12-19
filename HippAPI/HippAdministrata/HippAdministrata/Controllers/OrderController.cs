using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.Requests;
using HippAdministrata.Services;
using HippAdministrata.Services.Implementation;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDriverService _driverService;
        private readonly IClientService _clientService;
        private readonly IWarehouseService _warehouseService;
        private readonly IEmployeeService _employeeService;
        private readonly ISalesPersonService _salesPersonService;
        public OrderController(IOrderService orderService, ISalesPersonService salesPersonService, IDriverService driverService, IClientService clientService, IEmployeeService employeeService, IWarehouseService warehouseService)
        {
            _orderService = orderService;
            _driverService = driverService;
            _clientService = clientService;
            _warehouseService = warehouseService;
            _employeeService = employeeService;
            _salesPersonService = salesPersonService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetManagedOrders(int salesPersonId)
        {
            var orders = await _orderService.GetBySalesPersonIdAsync(salesPersonId);
            return Ok(orders);
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

        [HttpGet("salesperson/{salesPersonId}")]
        public async Task<IActionResult> GetBySalesPersonId(int salesPersonId)
        {
            var orders = await _orderService.GetBySalesPersonIdAsync(salesPersonId);
            return Ok(orders);
        }

       

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.Id) return BadRequest("Order ID mismatch.");

            var existingOrder = await _orderService.GetByIdAsync(id);
            if (existingOrder == null) return NotFound("Order not found.");

            var result = await _orderService.UpdateAsync(order);
            if (!result) return StatusCode(500, "Failed to update order.");
            return Ok("Order updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result) return NotFound("Order not found.");
            return Ok("Order deleted successfully.");
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result) return NotFound("Order not found or failed to update status.");
            return Ok("Order status updated successfully.");
        }
    }
}
