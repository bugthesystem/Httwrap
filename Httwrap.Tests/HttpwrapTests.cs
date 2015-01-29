using NUnit.Framework;
using Httwrap.Interface;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        protected override void FinalizeSetUp()
        {
            IHttwrapConfiguration configuration = new TestConfiguration("http://localapi/v1/");
            IHttwrapHttpClient restRequestClient = new HttwrapHttpClient(configuration);
        }

        [Test]
        public void Get_test()
        {
            Assert.IsTrue(true);
        }
    }
}
