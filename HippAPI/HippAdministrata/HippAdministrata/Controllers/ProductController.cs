using HippAdministrata.Models.Domains;
using HippAdministrata.Services;
using Microsoft.AspNetCore.Mvc;

namespace HippAdministrata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

    }
}
