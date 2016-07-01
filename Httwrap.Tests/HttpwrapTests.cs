using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Testing.NUnit;
using FluentAssertions;
using Httwrap.Auth;
using Httwrap.Interface;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        private IDisposable _server;
        private IHttwrapClient _client;
        private const string BaseAddress = "http://localhost:9000/";

        protected override async void FinalizeSetUp()
        {
            await ClearApiCache();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup>(BaseAddress);

            IHttwrapConfiguration configuration = new HttwrapConfiguration(BaseAddress);
            _client = new HttwrapClient(configuration);
        }

        [TestFixtureTearDown]
        protected void TestTearDown()
        {
            _server?.Dispose();
        }

        private async Task ClearApiCache()
        {
            var clearResponse = await _client.GetAsync("api/products?op=clear");
            clearResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public void BasicCredentials_should_set_auth_header_Test()
        {
            var credentials = new BasicAuthCredentials("user", "s3cr3t");
            var client = credentials.BuildHttpClient();
            client.DefaultRequestHeaders.Should().NotBeNullOrEmpty();

            var authHeader = client.DefaultRequestHeaders.FirstOrDefault(pair => pair.Key == "Authorization");

            authHeader.Should().NotBeNull();
            authHeader.Value.First().Should().Contain("Basic");
        }

        [Test]
        public async void Delete_test()
        {
            var product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            var response = await _client.DeleteAsync("api/products/1");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async void Get_All_test()
        {
            var product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            var response =
                await _client.GetAsync<IEnumerable<Product>>("api/products");

            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Count().Should().Be(1);
        }

        [Test]
        public async void Get_ById_ReadAs_test()
        {
            var product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            var response = await _client.GetAsync("api/products/1");
            var result = response.ReadAs<Product>();
            result.Should().NotBeNull();
        }

        [Test]
        public async void Get_ById_test()
        {
            var product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            var response = await _client.GetAsync<Product>("api/products/1");

            response.Data.Should().NotBeNull();
        }

        [Test]
        public async void Get_With_QueryString_test()
        {
            var payload = new FilterRequest
            {
                Category = "Shoes",
                NumberOfItems = 10
            };

            var response = await _client.GetAsync("api/test", payload);

            var expected = response.ReadAs<FilterRequest>();
            expected.Should().NotBeNull();
            expected.Category.Should().Be(payload.Category);
            expected.NumberOfItems.Should().Be(payload.NumberOfItems);
        }

        [Test]
        public void OAuth_with_username_password_should_set_auth_header_Test()
        {
            var credentials = new OAuthCredentials("us3r", "p4ssw0rd", BaseAddress + "api/authentication/token");
            var client = credentials.BuildHttpClient();
            client.DefaultRequestHeaders.Should().NotBeNullOrEmpty();
            var header = client.DefaultRequestHeaders.FirstOrDefault(pair => pair.Key == "Authorization");
            header.Should().NotBeNull();
            header.Value.First().Should().Contain("Bearer");
        }

        [Test]
        public void OAuthCredentials_should_set_auth_header_Test()
        {
            var credentials = new OAuthCredentials("token");
            var client = credentials.BuildHttpClient();
            client.DefaultRequestHeaders.Should().NotBeNullOrEmpty();

            var header = client.DefaultRequestHeaders.FirstOrDefault(pair => pair.Key == "Authorization");
            header.Should().NotBeNull();
            header.Value.First().Should().Contain("Bearer");
        }

        [Test]
        public async void Post_test()
        {
            var product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            var response = await _client.PostAsync("api/products", product);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async void Interceptor_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapConfiguration configuration = new HttwrapConfiguration(BaseAddress);
            HttwrapClient client = new HttwrapClient(configuration);

            client.AddInterceptor(new DummyInterceptor());
            var response = await client.GetAsync("api/products/1");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Accepted);
        }


        [Test]
        public void PostSync_Test()
        {
            Product product = FixtureRepository.Build<Product>().Without(item => item.Id).Create();

            IHttwrapResponse result = _client.Post("api/products", product);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            result.Should().NotBeNull();
        }

        [Test]
        public void GetAllSync_Test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();

            IHttwrapResponse savedProduct = _client.Post("api/products", product);

            IHttwrapResponse result = _client.Get("api/products", savedProduct);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public void DeleteSync_Test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();

            _client.Post("api/products", product);

            IHttwrapResponse result = _client.Delete("api/products/1");

            result.Should().NotBeNull();
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);
        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }
    }
}