using System;
using System.Net;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Ploeh.AutoFixture;
using Httwrap.Interface;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Collections.Generic;
using Httwrap.Auth;

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
            _server = WebApp.Start<Startup>(url: BaseAddress);

            IHttwrapConfiguration configuration = new HttwrapConfiguration(BaseAddress);
            _client = new HttwrapClient(configuration);
        }

        [Test]
        public async void Get_All_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapResponse<IEnumerable<Product>> response = await _client.GetAsync<IEnumerable<Product>>("api/products");

            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Count().Should().Be(1);
        }

        [Test]
        public async void Get_ById_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapResponse<Product> response = await _client.GetAsync<Product>("api/products/1");

            response.Data.Should().NotBeNull();
        }

        [Test]
        public async void Get_ById_ReadAs_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapResponse response = await _client.GetAsync("api/products/1");
            Product result = response.ReadAs<Product>();
            result.Should().NotBeNull();
        }

        [Test]
        public async void Post_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            IHttwrapResponse response = await _client.PostAsync("api/products", product);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async void Put_test()
        {
            const string PUT_TEST_PRODUCT_NAME = "Put Test Product";

            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            product.Name = PUT_TEST_PRODUCT_NAME;
            IHttwrapResponse putResponse = await _client.PutAsync("api/products/1", product);

            IHttwrapResponse<Product> getResponse = await _client.GetAsync<Product>("api/products/1");

            putResponse.Should().NotBeNull();
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.Should().NotBeNull();
            getResponse.Data.Should().NotBeNull();
            getResponse.Data.Name.ShouldBeEquivalentTo(PUT_TEST_PRODUCT_NAME);
        }


        [Test]
        public async void Delete_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapResponse response = await _client.DeleteAsync("api/products/1");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [Test]
        public void BasicCredentials_should_set_auth_header_Test()
        {
            BasicAuthCredentials credentials = new BasicAuthCredentials("user", "s3cr3t");
            var client = credentials.BuildHttpClient();
            client.DefaultRequestHeaders.Should().NotBeNullOrEmpty();

            var authHeader = client.DefaultRequestHeaders.FirstOrDefault(pair => pair.Key == "Authorization");

            authHeader.Should().NotBeNull();
            authHeader.Value.First().Should().Contain("Basic");
        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }

        private async Task ClearApiCache()
        {
            IHttwrapResponse clearResponse = await _client.GetAsync("api/products?op=clear");
            clearResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
