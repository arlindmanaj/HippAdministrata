using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Models.Requests;
using HippAdministrata.Services;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ClientController> _logger;
        private readonly ISalesPersonService _salesPersonService;
        private readonly ApplicationDbContext _context;

        public ClientController(
            IClientService clientService,
            IOrderService orderService,
            ILogger<ClientController> logger,
            ISalesPersonService salesPersonService,
            ApplicationDbContext context)
            
        {
            _clientService = clientService;
            _orderService = orderService;
            _logger = logger;
            _context = context;
            _salesPersonService = salesPersonService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound($"Client with ID {id} not found.");

            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

       

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client updatedClient)
        {
            var result = await _clientService.UpdateAsync(id, updatedClient);
            if (!result) return NotFound($"Client with ID {id} not found.");

            return Ok("Client updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clientService.DeleteAsync(id);
            if (!result) return NotFound($"Client with ID {id} not found.");

            return Ok("Client deleted successfully.");
        }

        [HttpPost("{clientId}/submit-order-request")]
        public async Task<IActionResult> SubmitOrderRequest(int clientId, [FromBody] ClientOrderRequestDto request)
        {
            try
            {
                var client = await _clientService.GetByIdAsync(clientId);
                if (client == null) return NotFound($"Client with ID {clientId} not found.");

                var salesPerson = await _context.SalesPersons.FirstOrDefaultAsync();
                if (salesPerson == null) return BadRequest("No SalesPerson available.");

                var salesPersonClient = new SalesPersonClients
                {
                    ClientId = clientId,
                    SalesPersonId = salesPerson.Id,
                    Name = request.Name,
                    DeliveryDestination = request.DeliveryDestination
                };

                await _context.SalesPersonClients.AddAsync(salesPersonClient);
                await _context.SaveChangesAsync();

                foreach (var product in request.Products)
                {
                    var productLink = new SalesPersonClientProduct
                    {
                        SalesPersonClientId = salesPersonClient.Id,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity
                    };
                    await _context.SalesPersonClientProducts.AddAsync(productLink);
                }

                await _context.SaveChangesAsync();
                return Ok("Order request submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the order request.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



    }
}
