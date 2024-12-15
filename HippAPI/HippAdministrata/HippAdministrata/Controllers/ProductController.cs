//using HippAdministrata.Models.Domains;
//using HippAdministrata.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace HippAdministrata.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductController : ControllerBase
//    {
//        private readonly ProductService _productService;

//        public ProductController(ProductService productService)
//        {
//            _productService = productService;
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetProductById(int id)
//        {
//            var product = await _productService.GetByIdAsync(id);
//            if (product == null) return NotFound();
//            return Ok(product);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllProducts()
//        {
//            var products = await _productService.GetAllAsync();
//            return Ok(products);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateProduct(Product product)
//        {
//            // Do not set product.Id manually, as it will be auto-generated.
//            if (await _productService.CreateAsync(product))
//                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
//            return BadRequest();
//        }


//        [HttpPut]
//        public async Task<IActionResult> UpdateProduct(Product product)
//        {
//            if (await _productService.UpdateAsync(product))
//                return NoContent();

//            return BadRequest("Failed to update product. It may not exist or was modified by another user.");
//        }


//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteProduct(int id)
//        {
//            if (await _productService.DeleteAsync(id))
//                return NoContent();
//            return NotFound();
//        }

//        [HttpPatch("{id}/quantities")]
//        public async Task<IActionResult> UpdateQuantities(int id, [FromBody] decimal labeled, decimal unlabeled)
//        {
//            if (await _productService.UpdateQuantitiesAsync(id, labeled, unlabeled))
//                return NoContent();
//            return BadRequest();
//        }
//    }
//}
