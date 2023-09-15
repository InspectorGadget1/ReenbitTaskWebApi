using Microsoft.AspNetCore.Mvc;
using Reenbit_Task.Services;

namespace Reenbit_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        private readonly IAzureBlobService blobService;

        public DocsController(IAzureBlobService blobService)
        {
            this.blobService = blobService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string userEmail)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest("Invalid file.");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".docx")
                {
                    return BadRequest("Only .docx files are allowed.");
                }

                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest("User email cannot be null or empty.");
                }

                _ = await blobService.UploadFileAsync(file, userEmail);

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
