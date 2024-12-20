using Microsoft.AspNetCore.Mvc;
using Softxpertise.API.EmailHandler.Services;
using System.ComponentModel.DataAnnotations;


namespace Softxpertise.API.EmailHandler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendContactForm([FromBody] ContactFormRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserEmail) || string.IsNullOrWhiteSpace(request.UserMessage))
            {
                return BadRequest("Invalid contact form submission.");
            }

            try
            {
                await _emailService.SendContactFormAsync(request.UserName, request.UserEmail, request.UserMessage);
                return Ok("Contact form submitted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error submitting contact form: {ex.Message}");
            }
        }
    }

    public class ContactFormRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserMessage { get; set; }
    }

    public class ContactFormModel
	{
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        public required string Message { get; set; }
	}
}
