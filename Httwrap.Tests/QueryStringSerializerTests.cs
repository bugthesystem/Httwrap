using FluentAssertions;
using NUnit.Framework;

namespace Httwrap.Tests
{
    public class QueryStringSerializerTests : TestBase
    {
        private IQueryStringSerializer _queryStringSerializer;

        protected override void FinalizeSetUp()
        {
            _queryStringSerializer = new QueryStringSerializer();
        }

        [Test]
        public void Serialize_Basic_Test()
        {
            string serialize = _queryStringSerializer.Serialize(new FilterRequest { Category = "Shoes", NumberOfItems = 20 });

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("Category=Shoes&NumberOfItems=20");
        }

        [Test]
        public void Serialize_Test()
        {
            string serialize = _queryStringSerializer.Serialize(new { Category = "Shoes", NumberOfItems = 20, Ids = new[] { 10, 20, 30 } });

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("Category=Shoes&NumberOfItems=20&Ids=10%2C20%2C30");
        }
    }
}