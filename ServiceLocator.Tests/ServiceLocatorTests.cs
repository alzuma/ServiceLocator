using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Shouldly;
using WebApplicationTest;
using Xunit;

namespace ServiceLocator.Tests
{
    public class ServiceLocatorTests
    {
        private readonly HttpClient _client;

        public ServiceLocatorTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task ScopedServiceTest()
        {
            var response = await _client.GetAsync("/api/values");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

            resultList.Count.ShouldBe(2);
            resultList.ShouldContain("value1");
            resultList.ShouldContain("value2");
        }
        
        [Fact]
        public async Task ScopedByTypeServiceTest()
        {
            var response = await _client.GetAsync("/api/values/bytype");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

            resultList.Count.ShouldBe(1);
            resultList.ShouldContain("value1");
        }

        [Fact]
        public async Task SingletonServiceTest()
        {
            var response = await _client.GetAsync("/api/values/singleton");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

            resultList.Count.ShouldBe(2);
            resultList.ShouldContain("value1");
            resultList.ShouldContain("value2");
        }

        [Fact]
        public async Task TransientServiceTest()
        {
            var response = await _client.GetAsync("/api/values/transient");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

            resultList.Count.ShouldBe(1);
            resultList.ShouldContain("value1");
        }

        [Fact]
        public async Task SameInterfaceTest()
        {
            var response = await _client.GetAsync("/api/same");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            
            responseString.ShouldBe("2");
        }
    }
}
