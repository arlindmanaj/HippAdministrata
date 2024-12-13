using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using HippAdministrata.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly DriverService _driverService;

        public DriverController(DriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var driver = await _driverService.GetByIdAsync(id);
            if (driver == null) return NotFound();
            return Ok(driver);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllAsync();
            return Ok(drivers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver(Driver driver)
        {
            if (await _driverService.CreateAsync(driver))
                return CreatedAtAction(nameof(GetDriverById), new { id = driver.Id }, driver);
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            if (await _driverService.UpdateAsync(driver))
                return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            if (await _driverService.DeleteAsync(id))
                return NoContent();
            return NotFound();
        }
    }
}
