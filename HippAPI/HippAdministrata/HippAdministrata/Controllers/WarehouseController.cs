using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null) return NotFound();
            return Ok(warehouse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses = await _warehouseService.GetAllAsync();
            return Ok(warehouses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(Warehouse warehouse)
        {
            if (await _warehouseService.CreateAsync(warehouse))
                return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.Id }, warehouse);
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWarehouse(Warehouse warehouse)
        {
            if (await _warehouseService.UpdateAsync(warehouse))
                return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            if (await _warehouseService.DeleteAsync(id))
                return NoContent();
            return NotFound();
        }
    }
}
