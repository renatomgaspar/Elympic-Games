using Elympic_Games.Mobile.Models;
using System.Net.Http;
using System.Text.Json;

namespace Elympic_Games.Mobile.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public EventService()
        {
            // Ignora certificado self-signed em desenvolvimento
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);

            // URL da API conforme a plataforma
            if (DeviceInfo.Platform == DevicePlatform.Android)
                _baseUrl = "https://10.0.2.2:7175/api/events";
            else
                _baseUrl = "https://localhost:7175/api/events";
        }

        public async Task<List<Event>> GetAllEvent()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return events ?? new List<Event>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching events: {ex.Message}");
                return new List<Event>();
            }
        }
    }
}
