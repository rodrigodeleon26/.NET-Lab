using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class PayPalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _httpClient;

    public PayPalService(IConfiguration configuration, HttpClient httpClient)
    {
        var payPalConfig = configuration.GetSection("PayPal");
        _clientId = payPalConfig["ClientId"];
        _clientSecret = payPalConfig["ClientSecret"];
        _httpClient = httpClient;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var authUrl = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
        var authRequest = new HttpRequestMessage(HttpMethod.Post, authUrl);
        authRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));
        authRequest.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        try
        {
            var response = await _httpClient.SendAsync(authRequest);

            if (!response.IsSuccessStatusCode)
            {
                // Lee el contenido del error
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener el token de acceso. Código de estado: {response.StatusCode}, Contenido del error: {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<PayPalAccessTokenResponse>(responseBody);

            Console.WriteLine($"Consiguio el TokenResponse: {tokenResponse.AccessToken}");

            if (tokenResponse == null)
            {
                throw new Exception("El formato de la respuesta de PayPal no es válido.");
            }

            return tokenResponse.AccessToken;
        }
        catch (HttpRequestException httpEx)
        {
            throw new Exception($"Error de red al intentar conectarse a PayPal: {httpEx.Message}", httpEx);
        }
        catch (JsonException jsonEx)
        {
            throw new Exception($"Error al procesar la respuesta JSON de PayPal: {jsonEx.Message}", jsonEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al intentar obtener el token de acceso: {ex.Message}", ex);
        }
    }

    public async Task<PayPalOrderResponse> CreateOrderAsync(
    List<PayPalPurchaseUnit> purchaseUnits,
    string currency,
    string returnUrl,
    string cancelUrl)
    {
        var accessToken = await GetAccessTokenAsync();

        foreach (var unit in purchaseUnits)
        {
            if (string.IsNullOrEmpty(unit.reference_id))
            {
                unit.reference_id = Guid.NewGuid().ToString(); // Generar un identificador único
            }
        }

        // Construir el objeto de solicitud con la lista de unidades de compra
        var orderRequest = new PayPalOrderRequest
        {
            purchase_units = purchaseUnits,
            intent = "CAPTURE",
            application_context = new PayPalApplicationContext
            {
                ReturnUrl = returnUrl,
                CancelUrl = cancelUrl
            }
        };

        var jsonRequest = JsonSerializer.Serialize(orderRequest);

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v2/checkout/orders")
        {
            Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al crear la orden de PayPal: {responseBody}");
        }

        return JsonSerializer.Deserialize<PayPalOrderResponse>(responseBody);
    }

    public async Task<PayPalCaptureResponse> CaptureOrderAsync(string orderId)
    {
        var accessToken = await GetAccessTokenAsync();

        // Configurar la solicitud HTTP
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); // Header obligatorio
        request.Content = new StringContent("{}", Encoding.UTF8, "application/json"); // Payload vacío pero válido JSON

        // Enviar la solicitud
        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Asegurarse de que la respuesta sea exitosa
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al capturar la orden de PayPal: {responseBody}");
        }

        // Parsear la respuesta
        var captureResponse = JsonSerializer.Deserialize<PayPalCaptureResponse>(responseBody);
        return captureResponse;
    }

    public async Task<PayPalOrderResponse> GetOrderDetailsAsync(string orderId)
    {
        var accessToken = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al obtener los detalles de la orden de PayPal: {responseBody}");
        }

        return JsonSerializer.Deserialize<PayPalOrderResponse>(responseBody);
    }
}

public class PayPalAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("app_id")]
    public string AppId { get; set; }

    [JsonPropertyName("nonce")]
    public string Nonce { get; set; }
}

public class PayPalPaymentResponse
{
    public string Id { get; set; }
    public string Intent { get; set; }
    public List<PayPalLink> Links { get; set; }
}

public class PayPalLink
{
    public string href { get; set; }
    public string rel { get; set; }
    public string method { get; set; }
}

public class PayPalTransaction
{
    public PayPalAmount Amount { get; set; }
    public string Description { get; set; }
}

//public class PayPalAmount
//{
//    public string Total { get; set; }
//    public string Currency { get; set; }
//}

public class PayPalRedirectUrls
{
    public string ReturnUrl { get; set; }
    public string CancelUrl { get; set; }
}

public class PayPalPaymentRequest
{
    public string Intent { get; set; }
    public PayPalPayer Payer { get; set; }
    public List<PayPalTransaction> Transactions { get; set; }
    public PayPalRedirectUrls RedirectUrls { get; set; }
}

public class PayPalPayer
{
    public string PaymentMethod { get; set; }
}

public class PayPalOrderRequest
{
    public List<PayPalPurchaseUnit> purchase_units { get; set; }
    public string intent { get; set; } = "CAPTURE";
    public PayPalApplicationContext application_context { get; set; }
}

public class PayPalPurchaseUnit
{
    public string reference_id { get; set; } // Identificador único para la unidad de compra
    public PayPalAmount amount { get; set; }
    public string description { get; set; }
}

public class PayPalAmount
{
    public string currency_code { get; set; }
    public string value { get; set; }
}

public class PayPalOrderResponse
{
    public string id { get; set; }
    public string status { get; set; }
    public List<PayPalLink> links { get; set; }
}

public class PayPalApplicationContext
{
    public string ReturnUrl { get; set; }
    public string CancelUrl { get; set; }
}

public class PayPalCaptureResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public PayPalAmount Amount { get; set; }
    public List<PayPalLink> Links { get; set; }
}