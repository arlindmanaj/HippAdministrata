using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services.Implementation
{
    public class SalesPersonClientService : ISalesPersonClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public SalesPersonClientService(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // Fetch all pending orders
        public async Task<IEnumerable<SalesPersonClientsDto>> GetAllPendingAsync()
        {
            var salesPersonClients = await _context.SalesPersonClients
                .Include(spc => spc.Client)
                .Include(spc => spc.SalesPerson)
                .ToListAsync();

            return salesPersonClients.Select(spc => new SalesPersonClientsDto
            {
                Id = spc.Id,
                SalesPersonId = spc.SalesPersonId,
                ClientId = spc.ClientId,
                Name = spc.Name,
                Quantity = spc.Quantity,
                DeliveryDestination = spc.DeliveryDestination,
                CreatedAt = spc.CreatedAt,
                SalesPersonName = spc.SalesPerson?.Username,
                ClientName = spc.Client?.Name
            });
        }

        // Process a client order request
        public async Task<bool> ProcessOrderRequestAsync(int requestId, OrderProcessRequestDto request)
        {
            // Fetch the client order request
            var salesPersonClient = await _context.SalesPersonClients
                .Include(spc => spc.Client)
                .FirstOrDefaultAsync(spc => spc.Id == requestId);

            if (salesPersonClient == null) throw new ArgumentException($"Order request with ID {requestId} not found.");

            // Create the finalized order
            var order = new Order
            {
                Name = salesPersonClient.Name,
                Quantity = salesPersonClient.Quantity,
                DeliveryDestination = salesPersonClient.DeliveryDestination,
                ClientId = salesPersonClient.ClientId,
                SalesPersonId = salesPersonClient.SalesPersonId,
                EmployeeId = request.EmployeeId,
                DriverId = request.DriverId,
                WarehouseId = request.WarehouseId,
                OrderStatus = request.OrderStatus,
                LastUpdated = DateTime.UtcNow
            };

            // Save the finalized order
            var orderResult = await _orderService.CreateAsync(order);
            if (!orderResult) return false;

            // Remove the processed request from SalesPersonClients
            _context.SalesPersonClients.Remove(salesPersonClient);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
