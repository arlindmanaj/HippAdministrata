﻿using HippAdministrata.Models.Domains;
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


       


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result) return NotFound("Order not found.");
            return Ok("Order deleted successfully.");
        }

       
    }
}
