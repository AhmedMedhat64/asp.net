using MainProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProductsController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("")]

        public ActionResult<int> CreateProduct(Product product)
        {
            product.Id = 0;
            _dbcontext.Set<Product>().Add(product);
            _dbcontext.SaveChanges();
            return Ok(product.Id);
        }
    }
}
