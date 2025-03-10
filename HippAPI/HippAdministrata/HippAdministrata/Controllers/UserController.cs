﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HippAdministrata.Services.Interface;
using HippAdministrata.Models.Requests;
using HippAdministrata.Models.Responses;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.Domains;
using HippAdministrata.Services.Implementation;
using HippAdministrata.Services;
using System.Security.Claims;

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
        [HttpGet("getCurrentUserId")]
        public IActionResult GetCurrentUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(userId);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _userService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _userService.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("salespersons")]
        public async Task<IActionResult> GetAllSalesPersons()
        {
            var salespersons = await _userService.GetAllSalesPersonsAsync();
            return Ok(salespersons);
        }
        [HttpGet("clients")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _userService.GetAllClientsAsync();
            return Ok(clients);
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