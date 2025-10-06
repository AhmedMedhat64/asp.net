using MainProject.Authrization;
using MainProject.Data;
using MainProject.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MainProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext dbcontext, 
            ILogger<ProductsController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct([FromQuery] Product product,
            [FromQuery(Name = "p2")] Product product2,
            Product product1,
            [FromHeader(Name = "Accept-Language")] string langauge,
            [FromHeader(Name = "Date")] string date)
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
        [Route("list")]
        //[Authorize(Policy = "SuperUsersOnly")]
        [Authorize(Policy = "EmployeesOnly")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            //var isAdmin = User.IsInRole("Admin");
            var userName = User.Identity.Name;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var records = _dbcontext.Set<Product>().ToList();
            return Ok(records);
        }
        [HttpGet]
        [Route("id")]
        [LogSensitiveAction]
        public ActionResult GetById(int id)   
        {
            _logger.LogDebug("Product #{id} not fount: " + id);
            var record = _dbcontext.Set<Product>().Find(id);
            if (record == null)
                _logger.LogWarning("Product #{id} not fount: " + id);
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
