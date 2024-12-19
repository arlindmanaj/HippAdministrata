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
        public OrderController(IOrderService orderService, IDriverService driverService, IClientService clientService, IEmployeeService employeeService, IWarehouseService warehouseService)
        {
            _orderService = orderService;
            _driverService = driverService;
            _clientService = clientService;
            _warehouseService = warehouseService;
            _employeeService = employeeService;
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

        [HttpPut("orders/{orderId}/assign-employee")]
        public async Task<IActionResult> AssignEmployee(int orderId, [FromBody] int employeeId)
        {
            var result = await _orderService.AssignEmployeeToOrder(orderId, employeeId);
            if (!result) return BadRequest("Failed to assign employee.");
            return Ok("Employee assigned successfully.");
        }

        [HttpPut("orders/{orderId}/assign-driver")]
        public async Task<IActionResult> AssignDriver(int orderId, [FromBody] int driverId)
        {
            var result = await _orderService.AssignDriverToOrder(orderId, driverId);
            if (!result) return BadRequest("Failed to assign driver.");
            return Ok("Driver assigned successfully.");
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

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        //{
        //    // Validate related entities
        //    var driver = await _driverService.GetByIdAsync(request.DriverId);
        //    if (driver == null) return BadRequest($"Driver with ID {request.DriverId} not found.");

        //    var client = await _clientService.GetByIdAsync(request.ClientId);
        //    if (client == null) return BadRequest($"Client with ID {request.ClientId} not found.");

        //    var salesPerson = await _salesPersonService.GetByIdAsync(request.SalesPersonId);
        //    if (salesPerson == null) return BadRequest($"SalesPerson with ID {request.SalesPersonId} not found.");

        //    var employee = await _employeeService.GetByIdAsync(request.EmployeeId);
        //    if (employee == null) return BadRequest($"Employee with ID {request.EmployeeId} not found.");

        //    var warehouse = request.WarehouseId.HasValue
        //        ? await _warehouseService.GetByIdAsync(request.WarehouseId.Value)
        //        : null;

        //    // Map the DTO to the domain model
        //    var order = new Order
        //    {
        //        Name = request.Name,
        //        Quantity = request.Quantity,
        //        DeliveryDestination = request.DeliveryDestination,
        //        ClientId = request.ClientId,
        //        SalesPersonId = request.SalesPersonId,
        //        EmployeeId = request.EmployeeId,
        //        DriverId = request.DriverId,
        //        WarehouseId = request.WarehouseId,
        //        OrderStatus = request.OrderStatus,
        //        LastUpdated = DateTime.UtcNow // Automatically set timestamp
        //    };

        //    var result = await _orderService.CreateAsync(order);
        //    if (!result) return StatusCode(500, "Failed to create order.");

        //    return Ok("Order created successfully.");
        //}

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
