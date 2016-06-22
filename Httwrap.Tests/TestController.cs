using System.Web.Http;

namespace Httwrap.Tests
{
    public class TestController : ApiController
    {
        [HttpGet]
        public FilterRequest FilterProducts([FromUri] FilterRequest request)
        {
            return request;
        }
    }
}