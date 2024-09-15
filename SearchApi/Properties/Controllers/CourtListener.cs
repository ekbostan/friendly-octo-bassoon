using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;  // For accessing configuration

namespace LegalApiProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LawDocumentsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _apiToken;

       
        public LawDocumentsController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            
            
            _apiBaseUrl = config["ExternalApi:BaseUrl"];
            _apiToken = config["ExternalApi:ApiToken"];
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments()
        {
        
            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl);

    
            request.Headers.Add("Authorization", $"Token {_apiToken}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPost("documents")]
        public async Task<IActionResult> CreateDocument([FromBody] object payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _apiBaseUrl)
            {
                Content = new StringContent(payload.ToString(), System.Text.Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Token {_apiToken}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
