using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Httwrap.Tests
{
    //REFERENCE: http://www.asp.net/web-api/overview/older-versions/creating-a-web-api-that-supports-crud-operations
    public class ProductsController : ApiController
    {
        private static readonly IProductRepository Repository = new ProductRepository();

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Repository.GetAll();
        }

        [HttpGet]
        public Product Get(int id)
        {
            var item = Repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        [HttpGet]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return Repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPost]
        public HttpResponseMessage PostProduct(Product item)
        {
            item = Repository.Add(item);
            var response = Request.CreateResponse(HttpStatusCode.Created, item);

            var uri = Url.Link("DefaultApi", new {id = item.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPut]
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!Repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [AcceptVerbs("Patch")]
        public string Patch(int id, Product product)
        {
            return $"PATCH:{id}";
        }

        [HttpDelete]
        public void DeleteProduct(int id)
        {
            var item = Repository.Get(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Repository.Remove(id);
        }

        [HttpGet]
        public IHttpActionResult ClearAll(string op)
        {
            if (op == "clear")
            {
                Repository.ClearAll();

                return Ok();
            }

            return BadRequest();
        }
    }
}