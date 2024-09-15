using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LegalApiProxy.Models;
using System.Text.Json;

namespace LegalApiProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PACERController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PACERController> _logger;
        private readonly string _apiBaseUrl = "https://www.courtlistener.com/api/rest/v3/";

        public PACERController(HttpClient httpClient, ILogger<PACERController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet("docket/{id}")]
        public async Task<IActionResult> GetDocket(int id)
        {
            var url = $"{_apiBaseUrl}dockets/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching docket with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching docket information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var docket = JsonSerializer.Deserialize<Docket>(jsonContent);

            return Ok(docket);
        }

        [HttpGet("docket/{id}/entries")]
        public async Task<IActionResult> GetDocketEntries(int id)
        {
            var url = $"{_apiBaseUrl}docket-entries/?docket={id}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching docket entries for docket ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching docket entries.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var docketEntries = JsonSerializer.Deserialize<List<DocketEntry>>(jsonContent);

            return Ok(docketEntries);
        }

        [HttpGet("document/{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var url = $"{_apiBaseUrl}recap-documents/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching document with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching document.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var document = JsonSerializer.Deserialize<Document>(jsonContent);

            return Ok(document);
        }

        [HttpGet("party/{id}")]
        public async Task<IActionResult> GetParty(int id)
        {
            var url = $"{_apiBaseUrl}parties/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching party with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching party information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var party = JsonSerializer.Deserialize<Party>(jsonContent);

            return Ok(party);
        }

        [HttpGet("attorney/{id}")]
        public async Task<IActionResult> GetAttorney(int id)
        {
            var url = $"{_apiBaseUrl}attorneys/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching attorney with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching attorney information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var attorney = JsonSerializer.Deserialize<Attorney>(jsonContent);

            return Ok(attorney);
        }
    }
}
