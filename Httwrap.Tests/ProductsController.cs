using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

namespace Httwrap.Tests
{
    //REFERENCE: http://www.asp.net/web-api/overview/older-versions/creating-a-web-api-that-supports-crud-operations
    public class ProductsController : ApiController
    {
        static readonly IProductRepository Repository = new ProductRepository();

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Repository.GetAll();
        }

        [HttpGet]
        public Product Get(int id)
        {
            Product item = Repository.Get(id);
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
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
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
            return string.Format("PATCH:{0}", id);
        }


        [HttpDelete]
        public void DeleteProduct(int id)
        {
            Product item = Repository.Get(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Repository.Remove(id);
        }

        [HttpGet]
        public void ClearAll(string op)
        {
            if (op == "clear")
                Repository.ClearAll();
        }
    }
}
