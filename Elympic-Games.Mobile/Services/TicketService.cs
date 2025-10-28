using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elympic_Games.Mobile.Services
{
    public class TicketService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public TicketService()
        {
            // Ignora certificado self-signed em desenvolvimento
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);

            // URL da API conforme a plataforma
            if (DeviceInfo.Platform == DevicePlatform.Android)
                _baseUrl = "https://10.0.2.2:7175/api/tickets";
            else
                _baseUrl = "https://localhost:7175/api/tickets";
        }

        public async Task<int> CheckTicketAvailable(string ticketId, int eventId, int mode)
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl+ "/checkticket?ticketId=" + ticketId+"&eventId="+eventId+"&mode="+mode);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var count = JsonSerializer.Deserialize<int>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching ticket count: {ex.Message}");
                return 0; // valor padrão em caso de erro
            }
        }

    }
}
