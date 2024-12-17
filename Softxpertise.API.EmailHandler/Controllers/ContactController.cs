using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;
using MailKit.Security;
using System.ComponentModel.DataAnnotations;
using System;

namespace Softxpertise.API.EmailHandler.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
	{
		[HttpPost("send")]
		public async Task<IActionResult> SendEmail([FromBody] ContactFormModel model)
		{
			if (ModelState.IsValid)
			{
				var email = new MimeMessage();
				email.From.Add(new MailboxAddress("Softxpertise", "corentin.beck@softxpertise.com"));
				email.To.Add(new MailboxAddress("Softxpertise", "contact@softxpertise.com"));
				email.Subject = model.Subject;
                email.Body = new TextPart("plain")
                {
                    Text = $@"
						Name: {model.Name}
						Email: {model.Email}
						Subject: {model.Subject}

						Message:
						{model.Message}
					"
                };

                using var smtp = new SmtpClient();
				try
				{
                    Console.WriteLine("Connecting to SMTP...");
                    await smtp.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                    Console.WriteLine("Connected.");
                    Console.WriteLine("Authenticating...");
                    // Récupération du mot de passe depuis la variable d'environnement
                    var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

                    // Vérification si le mot de passe est bien récupéré
                    if (string.IsNullOrEmpty(smtpPassword))
                    {
                        return StatusCode(500, "SMTP password not configured.");
                    }

                    await smtp.AuthenticateAsync("corentin.beck@softxpertise.com", smtpPassword);
                    Console.WriteLine("Authenticated.");
                    Console.WriteLine("Sending email...");
                    await smtp.SendAsync(email);
                    Console.WriteLine("Email sent successfully.");
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
