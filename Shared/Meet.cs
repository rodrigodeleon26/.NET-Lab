using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Shared
{
    public class Meet
    {
        private static readonly string _tokenEndpoint = "https://oauth2.googleapis.com/token";
        private static readonly string _clientId = "48134233839-ikthbqdo5edbjju2s0k0c90aab40n7f1.apps.googleusercontent.com";
        private static readonly string _clientSecret = "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";
        private static readonly string _redirectUri = "https://localhost/paciente/api/Pacientes/oauth2callback";

        public static async Task<(string accessToken, string refreshToken)> GetAccessToken(string code)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", _redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                });

                var response = await client.PostAsync(_tokenEndpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al obtener el token: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                string accessToken = tokenResponse.access_token?.ToString();
                string refreshToken = tokenResponse.refresh_token?.ToString();

                // Si refreshToken es null, retornarlo como null
                if (refreshToken == null)
                {
                    return (accessToken, null);
                }

                return (accessToken, refreshToken);
            }
        }

        public static async Task<string> RefreshAccessToken(string refreshToken)
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine("REFRESHING TOKEN");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                });

                var response = await client.PostAsync(_tokenEndpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al renovar el token: {response.StatusCode} - {response.ReasonPhrase}");
                }
                Console.WriteLine(response);

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
                Console.WriteLine(tokenResponse.access_token.ToString());
                return tokenResponse.access_token.ToString();
            }
        }


        public static async Task<GoogleMeetEventResult> CreateGoogleMeetEvent(string accessToken, DateTime startDateTime, DateTime endDateTime, string refreshToken)
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

                string newAccessToken = null;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Si el token expiró, renueva el token
                        newAccessToken = await RefreshAccessToken(refreshToken);

                        Console.WriteLine(newAccessToken);

                        // Reintenta la operación con el nuevo token
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                        response = await client.PostAsync(createEventEndpoint, jsonContent);

                        Console.WriteLine(response);

                        if (!response.IsSuccessStatusCode)
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Error al crear el evento con el nuevo token: {response.StatusCode} - {errorContent}");
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error al crear el evento: {response.StatusCode} - {errorContent}");
                    }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                var createdEvent = JsonConvert.DeserializeObject<dynamic>(responseContent);

                return new GoogleMeetEventResult
                {
                    HangoutLink = createdEvent.hangoutLink?.ToString() ?? "No se generó enlace de Google Meet.",
                    NewAccessToken = newAccessToken
                };
            }
        }
    }

    public class GoogleMeetEventResult
    {
        public string HangoutLink { get; set; }
        public string NewAccessToken { get; set; }
    }
}