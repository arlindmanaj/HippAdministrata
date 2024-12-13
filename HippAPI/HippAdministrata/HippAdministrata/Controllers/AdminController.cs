using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using HippAdministrata.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly DriverService _driverService;
        private readonly WarehouseService _warehouseService;
        //private readonly OrderHistoryService _orderHistoryService;

        public AdminController(
            ProductService productService,
            DriverService driverService,
            WarehouseService warehouseService)
          //  OrderHistoryService orderHistoryService)
        {
            _productService = productService;
            _driverService = driverService;
            _warehouseService = warehouseService;
            //_orderHistoryService = orderHistoryService;
        }

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
            if (await _warehouseService.DeleteAsync(id))
                return Ok("Warehouse deleted successfully");
            return NotFound("Warehouse not found");
        }

    }
}
