using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace ServiceLocator.Tests;

public class ServiceLocatorTests : IClassFixture<WebApplicationFactory<global::ServiceLocator.TestApi.Program>>
{
    private readonly HttpClient _client;

    public ServiceLocatorTests(WebApplicationFactory<global::ServiceLocator.TestApi.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ScopedServiceTest()
    {
        var response = await _client.GetAsync("/api/scoped");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

        resultList!.Count.ShouldBe(2);
        resultList.ShouldContain("value1");
        resultList.ShouldContain("value2");
    }
    
    [Fact]
    public async Task ScopedByTypeServiceTest()
    {
        var response = await _client.GetAsync("/api/scoped/bytype");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

        resultList!.Count.ShouldBe(1);
        resultList.ShouldContain("value1");
    }

    [Fact]
    public async Task SingletonServiceTest()
    {
        var response = await _client.GetAsync("/api/singleton");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var resultList = JsonConvert.DeserializeObject<List<string>>(responseString);

        resultList!.Count.ShouldBe(2);
        resultList.ShouldContain("value1");
        resultList.ShouldContain("value2");
    }

    [Fact]
    public async Task TransientServiceTest()
    {
        var response = await _client.GetAsync("/api/transient");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var service2Values = JsonConvert.DeserializeObject<List<string>>(responseString);
        service2Values.ShouldNotBeNull();
        service2Values!.Count.ShouldBe(0); // Transient - new instance has no values
    }

    [Fact]
    public async Task SameInterfaceTest()
    {
        var response = await _client.GetAsync("/api/same");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        
        responseString.ShouldBe("2");
    }

    [Fact]
    public async Task KeyedServiceBigTest()
    {
        var response = await _client.GetAsync("/api/keyed/big");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.ShouldContain("Big cache: test");
    }

    [Fact]
    public async Task KeyedServiceSmallTest()
    {
        var response = await _client.GetAsync("/api/keyed/small");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.ShouldContain("Small cache: test");
    }

    [Fact]
    public async Task KeyedServiceEnumerationTest()
    {
        var response = await _client.GetAsync("/api/keyed/all");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var caches = JsonConvert.DeserializeObject<List<string>>(responseString);

        caches.ShouldNotBeNull();
        caches!.Count.ShouldBe(2); // Both keyed caches enumerable
        caches.ShouldContain(c => c.Contains("Big cache"));
        caches.ShouldContain(c => c.Contains("Small cache"));
    }
}
