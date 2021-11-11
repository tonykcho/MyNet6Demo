using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;

namespace MyNet6Demo.Api.Tests.Functional
{
    [TestClass]
    public class BasicEndpointsTests
    {
        [TestMethod]
        public async Task BasicTest()
        {
            using var app = new BasicEndpointsTestsApp(x => { });

            var httpClient = app.CreateClient();

            var response = await httpClient.GetAsync("/");

            var responseText = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("Hello World!!", responseText);
        }
    }

    internal class BasicEndpointsTestsApp : WebApplicationFactory<Program>
    {
        private readonly Action<IServiceCollection> _serviceOverride;

        public BasicEndpointsTestsApp(Action<IServiceCollection> serviceOverride)
        {
            _serviceOverride = serviceOverride;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // builder.ConfigureServices();

            return base.CreateHost(builder);
        }
    }
}