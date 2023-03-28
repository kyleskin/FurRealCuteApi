using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace FurRealCute.Web.Api.Tests.Acceptance.Brokers;

public class FurRealCuteApiBroker
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;


    public FurRealCuteApiBroker(WebApplicationFactory<Program> webApplicationFactory, HttpClient httpClient)
    {
        _webApplicationFactory = webApplicationFactory;
        _httpClient = httpClient;
    }
}