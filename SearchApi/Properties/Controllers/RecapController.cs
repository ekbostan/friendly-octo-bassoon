using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using LegalApiProxy.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace LegalApiProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RECAPController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RECAPController> _logger;
        private readonly string _apiBaseUrl = "https://www.courtlistener.com/api/rest/v3/";

        public RECAPController(HttpClient httpClient, ILogger<RECAPController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> FetchPACERContent([FromBody] RecapFetchRequest request)
        {
            var url = $"{_apiBaseUrl}recap-fetch/";

            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching PACER content, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching PACER content.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var fetchResult = JsonSerializer.Deserialize<RecapStatusResponse>(jsonContent);

            return Ok(fetchResult);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPACERDocument([FromForm] RecapUploadRequest request)
        {
            var url = $"{_apiBaseUrl}recap/";

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(request.UploadType.ToString()), "upload_type");
            formData.Add(new StringContent(request.Court), "court");
            formData.Add(new StringContent(request.FilePathLocal), "filepath_local");
            formData.Add(new StringContent(request.PacerCaseId), "pacer_case_id");
            if (!string.IsNullOrEmpty(request.PacerDocId))
                formData.Add(new StringContent(request.PacerDocId), "pacer_doc_id");
            if (request.DocumentNumber.HasValue)
                formData.Add(new StringContent(request.DocumentNumber.Value.ToString()), "document_number");
            if (request.AttachmentNumber.HasValue)
                formData.Add(new StringContent(request.AttachmentNumber.Value.ToString()), "attachment_number");
            formData.Add(new StringContent(request.Debug.ToString()), "debug");

            var response = await _httpClient.PostAsync(url, formData);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error uploading PACER document, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error uploading PACER document.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var uploadResult = JsonSerializer.Deserialize<RecapStatusResponse>(jsonContent);

            return Ok(uploadResult);
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetRecapStatus(int id)
        {
            var url = $"{_apiBaseUrl}recap/{id}/";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching status for ID: {id}, Status: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching RECAP status.");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var statusResult = JsonSerializer.Deserialize<RecapStatusResponse>(jsonContent);

            return Ok(statusResult);
        }
    }
}
