using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Httwrap.Interface;
using Microsoft.Owin.Testing;
using NUnit.Framework;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        private TestServer _server;
        protected override void FinalizeSetUp()
        {
            //Demo purpose only.
            JExtensions.Serializer = new JsonSerializerWrapper();

            IHttwrapConfiguration configuration = new TestConfiguration("http://localapi/v1/");
            IHttwrapClient restRequestClient = new HttwrapClient(configuration);

            _server = TestServer.Create<Startup>();
        }

        [Test]
        public void Get_test()
        {
            Assert.IsTrue(true);
        }

        [TestFixtureTearDown]
        protected override void FinalizeTearDown()
        {
            _server.Dispose();
        }
    }
}