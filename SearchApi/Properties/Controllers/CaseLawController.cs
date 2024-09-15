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
    public class CaseLawController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CaseLawController> _logger;
        private readonly string _apiBaseUrl = "https://www.courtlistener.com/api/rest/v3/";

        public CaseLawController(HttpClient httpClient, ILogger<CaseLawController> logger)
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

        [HttpGet("cluster/{id}")]
        public async Task<IActionResult> GetCluster(int id)
        {
            var url = $"{_apiBaseUrl}clusters/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching cluster with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching cluster information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var cluster = JsonSerializer.Deserialize<Cluster>(jsonContent);

            return Ok(cluster);
        }

        [HttpGet("opinion/{id}")]
        public async Task<IActionResult> GetOpinion(int id)
        {
            var url = $"{_apiBaseUrl}opinions/{id}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching opinion with ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching opinion information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var opinion = JsonSerializer.Deserialize<Opinion>(jsonContent);

            return Ok(opinion);
        }

        [HttpGet("court/{courtId}")]
        public async Task<IActionResult> GetCourt(string courtId)
        {
            var url = $"{_apiBaseUrl}courts/{courtId}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching court with ID: {courtId}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching court information.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var court = JsonSerializer.Deserialize<Court>(jsonContent);

            return Ok(court);
        }
    }
}
