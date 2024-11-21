using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PayPalService _payPalService;

    public PaymentsController(PayPalService payPalService)
    {
        _payPalService = payPalService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] PaymentRequest request)
    {
        try
        {
            PayPalOrderResponse order = await _payPalService.CreateOrderAsync(
                request.value,
                "USD", // Moneda: Pesos Uruguayos
                "https://localhost:4200/cliente/payment/success", // URL éxito
                "https://localhost:4200/cliente/payment/cancel"   // URL cancelación
            );

            Console.WriteLine($"El infame ORDER del CONTROLLER: {JsonSerializer.Serialize(order)}");

            var approvalUrl = order.links.FirstOrDefault(link => link.rel == "approve")?.href;
            if (approvalUrl == null)
            {
                return BadRequest("No se pudo obtener la URL de aprobación.");
            }

            return Ok(new { redirectUrl = approvalUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("capture")]
    public async Task<IActionResult> CaptureOrder([FromBody] CaptureRequest request)
    {
        try
        {
            var capture = await _payPalService.CaptureOrderAsync(request.OrderId);
            return Ok(capture);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class PaymentRequest
{
    public string value { get; set; }
}

public class ExecutePaymentRequest
{
    public string PaymentId { get; set; }
    public string PayerId { get; set; }
}

public class CaptureRequest
{
    public string OrderId { get; set; }
}