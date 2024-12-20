using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Softxpertise.API.EmailHandler.Services
{
    public class EmailService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _fixedEmail;

        public EmailService(string tenantId, string clientId, string clientSecret, string fixedEmail)
        {
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _graphClient = new GraphServiceClient(clientSecretCredential);
            _fixedEmail = fixedEmail; // Votre adresse email fixe pour envoyer les messages
        }

        public async Task SendContactFormAsync(string userName, string userSubject, string userEmail, string userMessage)
        {
            var message = new Message
            {
                Subject = "New Contact Form Submission",
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = $@"
                    <h3>New Contact Form Submission</h3>
                    <p><strong>Name:</strong> {userName}</p>
                    <p><strong>Subject:</strong> {userSubject}</p>
                    <p><strong>Email:</strong> {userEmail}</p>
                    <p><strong>Message:</strong></p>
                    <p>{userMessage}</p>
                "
                },
                ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = _fixedEmail // L'email de destination
                        }
                    }
                }
            };

            Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody body = new()
            {
                Message = message,
                SaveToSentItems = false  // or true, as you want
            };

            try
            {
                await _graphClient.Users[_fixedEmail]
                .SendMail
                .PostAsync(body);
            }
            catch (Exception ex)
            {
                // log your exception here
            }
        }
    }
}
