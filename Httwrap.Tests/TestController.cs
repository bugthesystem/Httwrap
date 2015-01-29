using System.Collections.Generic;
using System.Web.Http;

namespace Httwrap.Tests
{
    public class TestController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "hello", "world" };
        }

        public string Get(int id)
        {
            return "hello world";
        }
    }
}