using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;

namespace Softxpertise.API.EmailHandler.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactController : ControllerBase
	{
		[HttpPost("send")]
		public async Task<IActionResult> SendEmail([FromBody] ContactFormModel model)
		{
			if (ModelState.IsValid)
			{
				var email = new MimeMessage();
				email.From.Add(new MailboxAddress("Softxpertise", "contact@softxpertise.com"));
				email.To.Add(new MailboxAddress("Softxpertise", "contact@softxpertise.com"));
				email.Subject = model.Subject;
				email.Body = new TextPart("plain")
				{
					Text = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}"
				};

				using var smtp = new SmtpClient();
				try
				{
					await smtp.ConnectAsync("smtp-mail.outlook.com", 587, false);
					await smtp.AuthenticateAsync("contact@softxpertise.com", "");
					await smtp.SendAsync(email);
					await smtp.DisconnectAsync(true);
					return Ok("Email sent successfully.");
				}
				catch (Exception ex)
				{
					return StatusCode(500, $"Failed to send email: {ex.Message}");
				}
			}
			return BadRequest("Invalid data.");
		}
	}

	public class ContactFormModel
	{
		public required string Name { get; set; }
		public required string Email { get; set; }
		public required string Subject { get; set; }
		public required string Message { get; set; }
	}
}
