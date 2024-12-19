using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using HippAdministrata.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    public class AdminController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly DriverService _driverService;
        private readonly WarehouseService _warehouseService;
        private readonly ManagerService _managerService;
        private readonly EmployeeService _employeeService;  
        private readonly SalesPersonService _salesPersonService;

        public AdminController(
            ProductService productService,
            DriverService driverService,
            WarehouseService warehouseService,
            ManagerService managerService,
            EmployeeService employeeService
            ,
            SalesPersonService salesPersonService)

        {
            _productService = productService;
            _driverService = driverService;
            _warehouseService = warehouseService;
            _managerService = managerService;
            _employeeService = employeeService;
            _salesPersonService = salesPersonService;
        }

        // =======================
        // Products
        // =======================
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (await _productService.CreateAsync(product))
                return Ok("Product created successfully");
            return BadRequest("Failed to create product");
        }

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (await _productService.UpdateAsync(product))
                return Ok("Product updated successfully");
            return BadRequest("Failed to update product");
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await _productService.DeleteAsync(id))
                return Ok("Product deleted successfully");
            return NotFound("Product not found");
        }

        // =======================
        // Drivers
        // =======================
        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllAsync();
            return Ok(drivers);
        }

        [HttpPost("drivers")]
        public async Task<IActionResult> CreateDriver(Driver driver)
        {
            if (await _driverService.CreateAsync(driver))
                return Ok("Driver created successfully");
            return BadRequest("Failed to create driver");
        }

        [HttpPut("drivers")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            if (await _driverService.UpdateAsync(driver))
                return Ok("Driver updated successfully");
            return BadRequest("Failed to update driver");
        }

        [HttpDelete("drivers/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            if (await _driverService.DeleteAsync(id))
                return Ok("Driver deleted successfully");
            return NotFound("Driver not found");
        }

        // =======================
        // Warehouses
        // =======================
        [HttpGet("warehouses")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses = await _warehouseService.GetAllAsync();
            return Ok(warehouses);
        }

        [HttpPost("warehouses")]
        public async Task<IActionResult> CreateWarehouse(Warehouse warehouse)
        {
            if (await _warehouseService.CreateAsync(warehouse))
                return Ok("Warehouse created successfully");
            return BadRequest("Failed to create warehouse");
        }

        [HttpPut("warehouses")]
        public async Task<IActionResult> UpdateWarehouse(Warehouse warehouse)
        {
            if (await _warehouseService.UpdateAsync(warehouse))
                return Ok("Warehouse updated successfully");
            return BadRequest("Failed to update warehouse");
        }

        [HttpDelete("warehouses/{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            bool isDeleted = await _warehouseService.DeleteAsync(id);

            if (!isDeleted)
            {
                // If deletion failed, check if it was because of orders
                var orders = await _warehouseService.GetOrdersByWarehouseAsync(id);
                if (orders.Any())
                {
                    return BadRequest(new { message = $"Cannot delete this warehouse because it has {orders.Count()} order(s) associated with it." });
                }

                return NotFound(new { message = "Warehouse not found" });
            }

            return Ok(new { message = "Warehouse deleted successfully" });
        }


        // =======================
        // Managers
        // =======================
        [HttpGet("managers")]
        public async Task<IActionResult> GetAllManagers()
        {
            var managers = await _managerService.GetAllAsync();
            return Ok(managers);
        }

        [HttpGet("managers/{id}")]
        public async Task<IActionResult> GetManagerById(int id)
        {
            var manager = await _managerService.GetByIdAsync(id);
            if (manager == null) return NotFound("Manager not found");
            return Ok(manager);
        }


        [HttpPut("managers/{id}")]
        public async Task<IActionResult> UpdateManager(int id, Manager updatedManager)
        {
            if (await _managerService.UpdateAsync(id, updatedManager))
                return Ok("Manager updated successfully");
            return BadRequest("Failed to update manager");
        }


        [HttpDelete("managers/{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            if (await _managerService.DeleteAsync(id))
                return Ok("Manager deleted successfully");
            return NotFound("Manager not found");
        }


        // =======================
        // Employee
        // =======================
        [HttpGet("employee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employee = await _employeeService.GetAllAsync();
            return Ok(employee);
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound("Employee not found");
            return Ok(employee);
        }


        [HttpPut("employee/{id}")]
        public async Task<IActionResult> UpdatedEmployee(int id, Employee updatedEmployee)
        {
            if (await _employeeService.UpdateAsync(id, updatedEmployee))
                return Ok("Employee updated successfully");
            return BadRequest("Failed to update employee");
        }


        [HttpDelete("employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (await _employeeService.DeleteAsync(id))
                return Ok("Employee deleted successfully");
            return NotFound("Employee not found");
        }

        // =======================
        // SalesPerson
        // =======================
        [HttpGet("SalesPerson")]
        public async Task<IActionResult> GetAllSalePerson()
        {
            var salesPerson = await _salesPersonService.GetAllAsync();
            return Ok(salesPerson);
        }

        [HttpGet("SalePerson/{id}")]
        public async Task<IActionResult> GetSalePersonById(int id)
        {
            var salesPerson = await _salesPersonService.GetByIdAsync(id);
            if (salesPerson == null) return NotFound("Employee not found");
            return Ok(salesPerson);
        }


        [HttpPut("SalePerson/{id}")]
        public async Task<IActionResult> UpdatedSalePerson(int id, SalesPerson updatedSalesPerson)
        {
            if (await _salesPersonService.UpdateAsync(id, updatedSalesPerson))
                return Ok("SalePerson updated successfully");
            return BadRequest("Failed to update SalePerson");
        }


        [HttpDelete("SalePerson/{id}")]
        public async Task<IActionResult> DeleteSalePerson(int id)
        {
            if (await _salesPersonService.DeleteAsync(id))
                return Ok("SalePerson deleted successfully");
            return NotFound("SalePerson not found");
        }
    }
}
