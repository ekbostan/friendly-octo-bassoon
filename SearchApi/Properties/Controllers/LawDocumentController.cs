using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetDocuments([FromQuery] List<string> urls)
        {
            if (urls == null || urls.Count == 0)
            {
                return BadRequest("Please provide at least one URL to fetch documents.");
            }

            var tasks = new List<Task<HttpResponseMessage>>();

            _logger.LogInformation("Starting to fetch {Count} documents concurrently.", urls.Count);

            foreach (var url in urls)
            {
                _logger.LogInformation("Sending request to {Url}", url);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", $"Token {Environment.GetEnvironmentVariable("API_TOKEN")}");
                tasks.Add(_httpClient.SendAsync(request));
            }

            var responses = await Task.WhenAll(tasks);

            var results = new List<object>();

            foreach (var response in responses)
            {
                string contentType = response.Content.Headers.ContentType.MediaType;
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successful response from {Url} with content type: {ContentType}", response.RequestMessage.RequestUri, contentType);
                }
                else
                {
                    _logger.LogWarning("Failed response from {Url} with status code: {StatusCode}", response.RequestMessage.RequestUri, response.StatusCode);
                }

                if (contentType == "application/json")
                {
                    results.Add(new { Url = response.RequestMessage.RequestUri.ToString(), Content = content });
                }
                else if (contentType == "text/html")
                {
                    _logger.LogWarning("Received HTML response from {Url}, parsing HTML.", response.RequestMessage.RequestUri);
                    var parsedHtml = ParseHtml(content);
                    results.Add(new { Url = response.RequestMessage.RequestUri.ToString(), Content = parsedHtml });
                }
                else
                {
                    _logger.LogError("Unexpected content type: {ContentType} from {Url}.", contentType, response.RequestMessage.RequestUri);
                    results.Add(new { Url = response.RequestMessage.RequestUri.ToString(), Content = content });
                }
            }

            return Ok(results);
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
