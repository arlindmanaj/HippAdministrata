using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Requests;
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

            public DriverController(IDriverService driverService)
            {
                _driverService = driverService;
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

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _driverService.DeleteAsync(id);
                if (!result) return NotFound();
                return Ok("Driver deleted successfully");
            }

            
            [HttpPut("{driverId}/order/{orderId}/ship")]
            public async Task<IActionResult> ShipOrder(int driverId, int orderId)
            {
                var result = await _driverService.ShipOrderAsync(driverId, orderId);
                if (!result) return BadRequest("Failed to ship order");
                return Ok("Order shipped successfully");
            }
            private static string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }

    }
   

}
