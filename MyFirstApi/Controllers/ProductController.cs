using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using MyFirstApi.Models;

namespace MyFirstApi.Models
{
    [Route("api/product")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product>{
            new Product { Id = 1, Name = "Laptop", Price = 999.99M },
            new Product { Id = 2, Name = "Smartphone", Price = 499.99M },
            new Product { Id = 3, Name = "Tablet", Price = 299.99M }
        };
        
        [HttpGet]
        public ActionResult<List<Product>> GetAllProducts()
        {
            return products;
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] Product newProduct)
        {
            newProduct.Id = products.Max(p => p.Id) + 1;
            products.Add(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products.Remove(product);
            return NoContent();
        }
    }
}
