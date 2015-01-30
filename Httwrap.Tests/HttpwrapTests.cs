using System;
using System.Collections.Generic;
using FluentAssertions;
using Httwrap.Interface;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Httwrap.Tests
{
    public class HttpwrapTests : TestBase
    {
        private IDisposable _server;
        private IHttwrapClient _httwrapClient;
        private const string BaseAddress = "http://localhost:9000/";

        protected override void FinalizeSetUp()
        {
            //Demo purpose only.
            JExtensions.Serializer = new JsonSerializerWrapper();

            IHttwrapConfiguration configuration = new TestConfiguration(BaseAddress);
            _httwrapClient = new HttwrapClient(configuration);

        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup>(url: BaseAddress);
        }

        [Test]
        public async void Get_test()
        {
            HttwrapResponse<List<string>> response = await _httwrapClient.GetAsync<List<string>>("api/values");

            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Count.Should().Be(2);
        }

        [TestFixtureTearDown]
        protected override void FinalizeTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }
    }
}