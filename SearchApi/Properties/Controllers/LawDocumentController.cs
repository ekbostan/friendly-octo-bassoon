using Microsoft.AspNetCore.Mvc;
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

        public LawDocumentsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments()
        {
            string externalApiUrl = "https://www.courtlistener.com/api/rest/v3/documents/";

            var request = new HttpRequestMessage(HttpMethod.Get, externalApiUrl);
            request.Headers.Add("Authorization", $"Token {Environment.GetEnvironmentVariable("API_TOKEN")}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            
            string contentType = response.Content.Headers.ContentType.MediaType;
            string content = await response.Content.ReadAsStringAsync();

            if (contentType == "application/json")
            {
                return Ok(content);
            }
            else if (contentType == "text/html")
            {
                var parsedHtml = ParseHtml(content);
                return Ok(parsedHtml);
            }
            else
            {
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
