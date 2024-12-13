using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HippAdministrata.Services.Interface;
using HippAdministrata.Models.Requests;
using HippAdministrata.Models.Responses;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.Domains;

namespace hippserver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User model)
        {
            var result = await _userService.AddUserAsync(model);

            if (!result)
                return BadRequest(new { message = "Error adding user" });

            return Ok(new { message = "User added successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] UpdateUserRequest model)
        {
            var result = await _userService.ChangeUserRoleAsync(id, model.Role, model.Name);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User role updated successfully" });
        }
    }

}