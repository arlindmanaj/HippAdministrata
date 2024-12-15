﻿using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .ToListAsync();
        }


        public async Task<bool> UpdateAsync(int id, Employee updatedEmployee)
        {
            var existingEmployee = await GetByIdAsync(id);
            if (existingEmployee == null) return false;

            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.Password = updatedEmployee.Password;
            existingEmployee.UserId = updatedEmployee.UserId;

            _context.Set<Employee>().Update(existingEmployee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee == null) return false;

            _context.Set<Employee>().Remove(employee);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
