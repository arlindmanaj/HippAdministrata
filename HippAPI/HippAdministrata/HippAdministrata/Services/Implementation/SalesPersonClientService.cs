using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services.Implementation
{
    public class SalesPersonClientService : ISalesPersonClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;

        public SalesPersonClientService(ApplicationDbContext context, IOrderService orderService, ILogger<SalesPersonClientService> logger)
        {
            _context = context;
            _logger = logger;
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
                DeliveryDestination = spc.DeliveryDestination,
                CreatedAt = spc.CreatedAt,
                SalesPersonName = spc.SalesPerson?.Username,
                ClientName = spc.Client?.Name
            });
        }

        // Process a client order request
        public async Task<bool> ProcessOrderRequestAsync(int requestId, OrderProcessRequestDto request)
        {
            try
            {
                _logger.LogInformation("Starting ProcessOrderRequestAsync for requestId: {RequestId}", requestId);

                // Load the SalesPersonClient with related data
                var salesPersonClient = await _context.SalesPersonClients
                    .Include(spc => spc.Products)
                    .ThenInclude(spcp => spcp.Product)
                    .FirstOrDefaultAsync(spc => spc.Id == requestId);

                if (salesPersonClient == null)
                {
                    _logger.LogWarning("SalesPersonClient with ID {RequestId} not found.", requestId);
                    return false;
                }

                _logger.LogInformation("SalesPersonClient loaded. Name: {Name}, ClientId: {ClientId}, ProductsCount: {ProductsCount}",
                    salesPersonClient.Name, salesPersonClient.ClientId, salesPersonClient.Products?.Count ?? 0);

                // Create a new order
                var order = new Order
                {
                    Name = salesPersonClient.Name,
                    Quantity = salesPersonClient.Products?.Sum(p => p.Quantity) ?? 0,
                    DeliveryDestination = salesPersonClient.DeliveryDestination,
                    LastUpdated = DateTime.UtcNow,
                    ClientId = salesPersonClient.ClientId,
                    SalesPersonId = salesPersonClient.SalesPersonId,
                    DriverId = request.DriverId,
                    EmployeeId = request.EmployeeId,
                    WarehouseId = request.WarehouseId,
                    OrderStatus = OrderStatus.Created
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order created successfully with ID: {OrderId}", order.Id);

                // Create OrderProduct entries
                if (salesPersonClient.Products != null)
                {
                    foreach (var spcProduct in salesPersonClient.Products)
                    {
                        var orderProduct = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = spcProduct.ProductId,
                            Quantity = spcProduct.Quantity
                        };

                        await _context.OrderProducts.AddAsync(orderProduct);
                        _logger.LogInformation("OrderProduct created: OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}",
                            order.Id, spcProduct.ProductId, spcProduct.Quantity);
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("No products found for SalesPersonClient ID {RequestId}", requestId);
                }

                _logger.LogInformation("Order processed successfully for requestId: {RequestId}", requestId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the order request for requestId: {RequestId}", requestId);
                throw; // Re-throw the exception to allow controller to handle it
            }
        }

    }
}
