using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;


        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Set<Order>().FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Set<Order>().ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await _context.Set<Order>().Where(o => o.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId)
        {
            return await _context.Set<Order>().Where(o => o.SalesPersonId == salesPersonId).ToListAsync();
        }

        public async Task<bool> CreateAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Set<Order>().Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Set<Order>().Remove(order);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                order.LastUpdated = DateTime.UtcNow;
                // Update the status field here (depends on your model structure).
                return await UpdateAsync(order);
            }
            return false;
        }
    }
}
