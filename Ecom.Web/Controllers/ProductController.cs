using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger) { 
            _logger = logger;
        }

        [HttpGet()]
        public ActionResult Index() {
            return View();
        }
    }
}
