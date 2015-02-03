using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Httwrap.Interface;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        private IDisposable _server;
        private IHttwrapClient _httwrapClient;
        private const string BaseAddress = "http://localhost:9000/";

        protected override async void FinalizeSetUp()
        {
            await ClearDb();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup>(url: BaseAddress);

            IHttwrapConfiguration configuration = new TestConfiguration(BaseAddress);
            _httwrapClient = new HttwrapClient(configuration);
        }

        [Test]
        public async void Get_All_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _httwrapClient.PostAsync("api/products", product);

            HttwrapResponse<IEnumerable<Product>> response = await _httwrapClient.GetAsync<IEnumerable<Product>>("api/products");

            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Count().Should().Be(1);
        }

        [Test]
        public async void Get_ById_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _httwrapClient.PostAsync("api/products", product);

            HttwrapResponse<Product> response = await _httwrapClient.GetAsync<Product>("api/products/1");

            response.Data.Should().NotBeNull();
        }

        [Test]
        public async void Post_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            HttwrapResponse response = await _httwrapClient.PostAsync("api/products", product);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        [Test]
        public async void Delete_test()
        {
            Product product = FixtureRepository.Build<Product>().Without(p => p.Id).Create();
            await _httwrapClient.PostAsync("api/products", product);

            HttwrapResponse response = await _httwrapClient.DeleteAsync("api/products/1");

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
            HttwrapResponse clearResponse = await _httwrapClient.GetAsync("api/products?op=clear");
            clearResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
