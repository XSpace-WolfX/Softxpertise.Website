using Softxpertise.Website.Models;
using System.Net.Http.Json;

namespace Softxpertise.Website.Services
{
    public class ContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool Success, string Message)> SubmitContactForm(ContactModel formModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Contact/send", formModel);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Form submitted successfully!");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, $"Error: {errorMessage}");
            }
            catch (Exception ex)
            {
                return (false, $"An exception occurred: {ex.Message}");
            }
        }
    }
}
