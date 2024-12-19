using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Orders)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Orders)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, Client updatedClient)
        {
            var existingClient = await GetByIdAsync(id);
            if (existingClient == null) return false;

            existingClient.Name = updatedClient.Name;
            existingClient.Email = updatedClient.Email;
            existingClient.Phone = updatedClient.Phone;
            existingClient.Address = updatedClient.Address;
            existingClient.UserId = updatedClient.UserId;

            _context.Clients.Update(existingClient);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await GetByIdAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
