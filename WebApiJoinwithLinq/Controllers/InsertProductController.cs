using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiJoinwithLinq.Data;
using WebApiJoinwithLinq.Model.DTO;

namespace WebApiJoinwithLinq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertProductController : ControllerBase
    {
        private readonly ApplicationDbcontext entity;
        public InsertProductController(ApplicationDbcontext context)
        {
            entity = context;
        }
        [HttpPost]
        [Route("InsertProduct")]
        public async Task<IActionResult> PostProduct(string name, int categoryid)
        {
            if (string.IsNullOrEmpty(name) || categoryid == 0)
            {
                return BadRequest("Name and CategoryId are required.");
            }
            else
            {
                var product = new Model.Entities.Product
                {
                    Name = name,
                    CategoryId = categoryid,
                    Category = await entity.Categories.FindAsync(categoryid)
                };
                entity.Products.Add(product);
                await entity.SaveChangesAsync();
                return CreatedAtAction(nameof(PostProduct), new { id = product.ProductId }, product);
            }
        }
        [HttpPost]
        [Route("InsertCategory")]
        public async Task<IActionResult> PostCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name is required.");
            }
            else
            {
                var category = new Model.Entities.Category
                {
                    Name = name
                };
                entity.Categories.Add(category);
                await entity.SaveChangesAsync();
                return CreatedAtAction(nameof(PostCategory), new { id = category.CategoryId }, category);
            }
        }
        [HttpGet]
        [Route("Getjoined")]
        public async Task<IActionResult> GetJoined(int pid)
        {
            if (pid == 0)
            {
                return BadRequest("Product id is invalid");
            }
            else
            {
                var product = await entity.Products.FindAsync(pid);
                if (product == null)
                {
                    return NotFound();
                }
                var result = await (
                     from p in entity.Products
                     join
                     c in entity.Categories on p.CategoryId equals c.CategoryId
                     where p.ProductId == pid
                     select new JoinedDTO
                     {
                         ProductId = p.ProductId,
                         ProductName = p.Name,
                         CategoryName = c.Name
                     }).FirstOrDefaultAsync();
                return Ok(result);
            }
        }
        [HttpGet]
        [Route("Getalljoined")]
        public async Task<IActionResult> GetAllJoined()
        {
            var result = await (
                from p in entity.Products
                join c in entity.Categories on
                p.CategoryId equals c.CategoryId
                select new JoinedDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    CategoryName = c.Name
                }
                ).ToListAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("No products found.");
            }
            return Ok(result);

        }
        [HttpGet]
        [Route("GetAllProductsByCategory")]
        public async Task<IActionResult> getallbycategory(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Category Id");
            }
            else
            {
                var cproducts = await entity.Categories.FindAsync(id);
                if (cproducts == null)
                {
                    return NotFound("Category Not Found");
                }
                var products = await (
                    from p in entity.Products
                    join
                    c in entity.Categories on
                    p.CategoryId equals c.CategoryId
                    where p.CategoryId == id
                    select new JoinedDTO
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Name,
                        CategoryName = c.Name
                    }
                    ).ToListAsync();
                return Ok(products);
            }
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Product Id");
            }
            else
            {
                var product = await entity.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Product Not Found");
                }
                entity.Products.Remove(product);
                await entity.SaveChangesAsync();
                return Ok("Product Deleted Successfully");
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateProduct(int id,string name,int categoryid)
        {
            if(id==0||string.IsNullOrEmpty(name))
            {
                return BadRequest("Invalid Update Request");
            }
            var product = await entity.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            product.Name = name;
            product.CategoryId = categoryid;
            await entity.SaveChangesAsync();
            return Ok("Product Updated Successfully");

        }
    }
}
