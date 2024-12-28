using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.Requests;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace HippAdministrata.Controllers
{
   // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
       
            private readonly IDriverService _driverService;
            private readonly IOrderService _orderService;
            private readonly IProductService _productService;
            public DriverController(IDriverService driverService, IProductService productService , IOrderService orderService)
            {
                _driverService = driverService;
                _orderService = orderService;
                _productService = productService;
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var driver = await _driverService.GetByIdAsync(id);
                if (driver == null) return NotFound();
                return Ok(driver);
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var drivers = await _driverService.GetAllAsync();
                return Ok(drivers);
            }

            [HttpGet("assigned-orders/{driverId}")]
            public async Task<IActionResult> GetAssignedOrders( int driverId)
            {
                try
                {
                    var orders = await _driverService.GetAssignedOrdersAsync(driverId);
                    return Ok(orders);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }




        [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] DriverUpdateRequest request)
            {
                if (id != request.Id)
                    return BadRequest("Driver ID mismatch.");

                var existingDriver = await _driverService.GetByIdAsync(id);
                if (existingDriver == null)
                    return NotFound("Driver not found.");

                // Update fields
                existingDriver.Name = request.Name;
                existingDriver.Password = HashPassword(request.Password);
                existingDriver.LicensePlate = request.LicensePlate;
                existingDriver.CarModel = request.CarModel;

                var result = await _driverService.UpdateAsync(existingDriver);
                if (!result)
                    return StatusCode(500, "Failed to update driver.");

                return Ok("Driver updated successfully.");
            }

            [HttpPost("{productId}/transfer")]
            public async Task<IActionResult> TransferProductBetweenWarehouses(int productId, [FromBody] TransferProductDto transferDto)
            {
                try
                {
                    await _driverService.TransferProductBetweenWarehouses(
                        productId,
                        transferDto.SourceWarehouseId,
                        transferDto.DestinationWarehouseId);

                    return Ok($"Product {productId} has been transferred successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _driverService.DeleteAsync(id);
                if (!result) return NotFound();
                return Ok("Driver deleted successfully");
            }


            [HttpPost("simulate-shipping/{orderId}")]
            public async Task<IActionResult> SimulateShipping(int driverId, int orderId)
            {
                try
                {
                    await _orderService.SimulateShippingAsync(driverId, orderId); // Invoke the service method
                    return Ok("Shipping simulation completed successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message); // Handle exceptions and provide feedback
                }
            }



        private static string HashPassword(string password)
        {
                using var sha256 = SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
        }

    }
   

}
