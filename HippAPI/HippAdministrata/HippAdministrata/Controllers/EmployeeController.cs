using HippAdministrata.Models.DTOs;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("label")]
        public async Task<IActionResult> LabelProduct([FromHeader] int employeeId,[FromBody] LabelingOrderDto labelingOrderDto)
        {
            try
            {
                await _employeeService.LabelOrderProductAsync(employeeId, labelingOrderDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("orders/{employeeId}")]
        public async Task<IActionResult> GetAssignedOrders(int employeeId)
        {
            try
            {
                var orders = await _employeeService.GetAssignedOrdersAsync(employeeId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}
