using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Requests;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ApplicationDbContext _context;

        public ClientService(IClientRepository clientRepository, ApplicationDbContext context)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

       
        public async Task<bool> CreateOrderRequestAsync(int clientId, ClientOrderRequestDto request)
        {
            // Validate client exists
            var client = await GetByIdAsync(clientId);
            if (client == null) throw new ArgumentException($"Client with ID {clientId} not found.");

            // Get the first available SalesPerson
            var salesPerson = await _context.SalesPersons.FirstOrDefaultAsync();
            if (salesPerson == null) throw new InvalidOperationException("No SalesPerson available.");

            // Create the order request
            var clientOrderRequest = new ClientOrderRequest
            {
                ClientId = clientId,
                Name = request.Name,
                
                DeliveryDestination = request.DeliveryDestination,
                SalesPersonId = salesPerson.Id
            };

            await _context.ClientOrderRequests.AddAsync(clientOrderRequest);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ClientOrderRequest>> GetClientOrderRequestsAsync()
        {
            return await _context.ClientOrderRequests
                .Include(cor => cor.Client)
                .Include(cor => cor.SalesPerson)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int id, Client updatedClient)
        {
            return await _clientRepository.UpdateAsync(id, updatedClient);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _clientRepository.DeleteAsync(id);
        }

        public async Task<bool> CreateOrderAsync(int clientId, Order order)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null) throw new ArgumentException($"Client with ID {clientId} not found.");

            order.ClientId = clientId;
            client.Orders ??= new List<Order>();
            client.Orders.Add(order);

            return await _clientRepository.UpdateAsync(clientId, client);
        }
    }
}
