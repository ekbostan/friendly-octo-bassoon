using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace LegalApiProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LawDocumentsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LawDocumentsController> _logger;

        public LawDocumentsController(HttpClient httpClient, ILogger<LawDocumentsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments()
        {
            string externalApiUrl = "https://www.courtlistener.com/api/rest/v3/documents/";

            _logger.LogInformation("Sending request to {ExternalApiUrl}", externalApiUrl);

            var request = new HttpRequestMessage(HttpMethod.Get, externalApiUrl);
            request.Headers.Add("Authorization", $"Token {Environment.GetEnvironmentVariable("API_TOKEN")}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string contentType = response.Content.Headers.ContentType.MediaType;
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successful response from external API with content type: {ContentType}", contentType);
            }
            else
            {
                _logger.LogWarning("Failed response from external API with status code: {StatusCode}", response.StatusCode);
            }

            if (contentType == "application/json")
            {
                _logger.LogInformation("Processing JSON response.");
                return Ok(content);
            }
            else if (contentType == "text/html")
            {
                _logger.LogWarning("Received HTML response, parsing HTML.");
                var parsedHtml = ParseHtml(content);
                return Ok(parsedHtml);
            }
            else
            {
                _logger.LogError("Unexpected content type: {ContentType}. Returning raw response.", contentType);
                return StatusCode((int)response.StatusCode, content);
            }
        }

        private string ParseHtml(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var titleNode = doc.DocumentNode.SelectSingleNode("//title");
            var h1Node = doc.DocumentNode.SelectSingleNode("//h1");

            string title = titleNode?.InnerText ?? "No Title";
            string errorMessage = h1Node?.InnerText ?? "No Error Message";

            _logger.LogInformation("Parsed HTML with title: {Title} and error message: {ErrorMessage}", title, errorMessage);

            var result = new
            {
                Title = title,
                ErrorMessage = errorMessage,
                OriginalHtml = htmlContent
            };

            return System.Text.Json.JsonSerializer.Serialize(result);
        }
    }
}
