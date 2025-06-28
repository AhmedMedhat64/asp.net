using MainProject.Data;
using MainProject.Filters;
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
        [HttpPut]
        [Route("")]
        public ActionResult UpdateProduct(Product product)
        {
            var existingProduct = _dbcontext.Set<Product>().Find(product.Id);
            if (existingProduct == null) {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;
            _dbcontext.Set<Product>().Update(existingProduct);
            _dbcontext.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var records = _dbcontext.Set<Product>().ToList();
            return Ok(records);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetById(int id)
        {
            var record = _dbcontext.Set<Product>().Find(id);
            return record == null ? NotFound() : Ok(record);
        }
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _dbcontext.Set<Product>().Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _dbcontext.Set<Product>().Remove(existingProduct);
            _dbcontext.SaveChanges();
            return Ok();
        }
    }
}
