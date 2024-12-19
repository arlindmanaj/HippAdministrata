using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Services;
using HippAdministrata.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                if (!products.Any()) return NotFound("No products found.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return NotFound($"Product with ID {id} not found.");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequestDto request)
        {
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    UnlabeledQuantity = request.UnlabeledQuantity,
                    LabeledQuantity = 0 // Default to 0, as labeling is processed later
                };

                var result = await _productService.CreateAsync(product);
                if (!result)
                    return BadRequest("Failed to create product.");

                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                if (!result) return BadRequest("Failed to delete product.");

                return Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

}
