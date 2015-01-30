using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Httwrap.Tests
{
    //TESTING: http://www.asp.net/web-api/overview/older-versions/creating-a-web-api-that-supports-crud-operations
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        static readonly IProductRepository Repository = new ProductRepository();

        public IEnumerable<Product> Get()
        {
            return Repository.GetAll();
        }

        public Product Get(int id)
        {
            Product item = Repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return Repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        public HttpResponseMessage PostProduct(Product item)
        {
            item = Repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;

        }

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