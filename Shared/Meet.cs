using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared
{
    public class Meet
    {
        private static readonly string _tokenEndpoint = "https://oauth2.googleapis.com/token";
        private static readonly string _clientId = "48134233839-ikthbqdo5edbjju2s0k0c90aab40n7f1.apps.googleusercontent.com";
        private static readonly string _clientSecret = "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";
        private static readonly string _redirectUri = "https://localhost:5001/api/Pacientes/oauth2callback";

        public static async Task<string> GetAccessToken(string code)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", _redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                });

                var response = await client.PostAsync(_tokenEndpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al obtener el token: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                return tokenResponse.access_token.ToString();
            }
        }

        public static async Task<string> CreateGoogleMeetEvent(string accessToken, DateTime startDateTime, DateTime endDateTime)
        {
            Console.WriteLine(accessToken);
            const string createEventEndpoint = "https://www.googleapis.com/calendar/v3/calendars/primary/events?conferenceDataVersion=1";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var eventData = new
                {
                    summary = "Consulta médica",
                    description = "Consulta médica",
                    start = new
                    {
                        dateTime = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), // Formato ISO 8601
                        timeZone = "America/Montevideo"
                    },
                    end = new
                    {
                        dateTime = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        timeZone = "America/Montevideo"
                    },
                    conferenceData = new
                    {
                        createRequest = new
                        {
                            requestId = Guid.NewGuid().ToString() // Un identificador único
                        }
                    }
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(createEventEndpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear el evento: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var createdEvent = JsonConvert.DeserializeObject<dynamic>(responseContent);

                return createdEvent.hangoutLink?.ToString() ?? "No se generó enlace de Google Meet.";
            }
        }


    }
}