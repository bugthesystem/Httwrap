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

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        private IDisposable _server;
        private IHttwrapClient _client;
        private const string BaseAddress = "http://localhost:9000/";

        protected override async void FinalizeSetUp()
        {
            await ClearDb();
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
        public async void Delete_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _client.PostAsync("api/products", product);

            IHttwrapResponse response = await _client.DeleteAsync("api/products/1");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }

        private async Task ClearDb()
        {
            IHttwrapResponse clearResponse = await _client.GetAsync("api/products?op=clear");
            clearResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
