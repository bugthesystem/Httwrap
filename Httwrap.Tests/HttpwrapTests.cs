using Httwrap.Interface;
using NUnit.Framework;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        protected override void FinalizeSetUp()
        {
            IHttwrapConfiguration configuration = new TestConfiguration("http://localapi/v1/");
            IHttwrapClient restRequestClient = new HttwrapClient(configuration);
        }

        [Test]
        public void Get_test()
        {
            Assert.IsTrue(true);
        }
    }
}