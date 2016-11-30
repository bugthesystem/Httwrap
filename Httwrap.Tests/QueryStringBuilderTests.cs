using Common.Testing.NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Httwrap.Tests
{
    public class QueryStringBuilderTests : TestBase
    {
        private IQueryStringBuilder _queryStringBuilder;

        protected override void FinalizeSetUp()
        {
            _queryStringBuilder = new QueryStringBuilder();
        }

        [Test]
        public void Serialize_Basic_Test()
        {
            var serialize = _queryStringBuilder.BuildFrom(new FilterRequest {Category = "Shoes", NumberOfItems = 20});

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("Category=Shoes&NumberOfItems=20");
        }

        [Test]
        public void Serialize_DataMember_Test()
        {
            var serialize = _queryStringBuilder.BuildFrom(new AttributeRequest {Category = "Shoes", NumberOfItems = 20});

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("cat=Shoes&NumberOfItems=20");
        }

        [Test]
        public void Serialize_IgnoreDataMember_Test()
        {
            var serialize =
                _queryStringBuilder.BuildFrom(new IgnoreAttributeRequest {Category = "Shoes", NumberOfItems = 20});

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("Category=Shoes");
        }

        [Test]
        public void Serialize_Test()
        {
            var serialize =
                _queryStringBuilder.BuildFrom(new {Category = "Shoes", NumberOfItems = 20, Ids = new[] {10, 20, 30}});

            serialize.Should().NotBeNullOrEmpty();
            serialize.Should().Be("Category=Shoes&NumberOfItems=20&Ids=10%2C20%2C30");
        }
    }
}