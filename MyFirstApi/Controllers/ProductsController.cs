using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyFirstApi.Controllers

{

    [ApiController]

    // The route for the api is based on the controller name (AppleController -> apple)
    [Route("api/[controller]")]
    public class AppleController : ControllerBase
    {
        // the route for this action is: api/apple/featured
        [HttpGet("featured")]
        public ActionResult<List<string>> Get()
        {
            return new List<string> { "Apple", "Banana", "Orange" };

        }
        // the route for this action is: api/apple
        public string GetFeaturedProduct() => "Fuji Apple";
        // the route for this action is: api/apple/kimchi
        [HttpGet("kimchi")]
        public string GetKimchiProduct() => "Cabbage Kimchi";

        // the route for this action is: api/apple and use post method
        [HttpPost]
        public ActionResult<string> Post([FromBody] string newProduct)
        {

            return $"Added: {newProduct}";

        }
        // the route for this action is: api/apple/{id} and use put method
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string updatedProduct)
        {

            return $"Updated product {id} to: {updatedProduct}";

        }
        // the route for this action is: api/apple/{id} and use delete method
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {

            return $"Deleted product with ID: {id}";

        }
    }

}