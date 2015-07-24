using System.Collections.Generic;

namespace Httwrap.Tests
{
    //REFERENCE: http://www.asp.net/web-api/overview/older-versions/creating-a-web-api-that-supports-crud-operations
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        Product Add(Product item);
        void Remove(int id);
        bool Update(Product item);
        void ClearAll();
    }
}