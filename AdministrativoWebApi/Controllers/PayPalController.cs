using BL.BLs;
using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PayPalService _payPalService;
    private readonly IBL_Administrativo _blAdministrativo;

    public PaymentsController(PayPalService payPalService, IBL_Administrativo blAdministrativo)
    {
        _payPalService = payPalService;
        _blAdministrativo = blAdministrativo;
    }

    [HttpGet("pagos")]
    public IActionResult GetPaypalPagos()
    {
        try
        {
            var pagos = _blAdministrativo.GetPaypalPagos();
            return Ok(pagos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("pagos/{id}")]
    public IActionResult GetPaypalPagoById(long id)
    {
        try
        {
            var pago = _blAdministrativo.GetPaypalPagoById(id);
            if (pago == null)
            {
                return NotFound("No se encontró el pago con el ID especificado.");
            }

            return Ok(pago);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("pagos/pororden/{id}")]
    public IActionResult GetPaypalPagoByOrdenId(string id)
    {
        try
        {
            var pago = _blAdministrativo.GetPaypalPagoByOrdenId(id);
            if (pago == null)
            {
                return NotFound("No se encontró el pago con el ID especificado.");
            }

            return Ok(pago);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("pagos/add")]
    public IActionResult AddPaypalPago([FromBody] PagoPayPal nuevoPago)
    {
        try
        {
            _blAdministrativo.AddPaypalPago(nuevoPago);
            return Ok("Pago registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] PaymentRequest request)
    {
        try
        {
            // Llamar al servicio PayPal con la lista de unidades de compra
            PayPalOrderResponse order = await _payPalService.CreateOrderAsync(
                request.PurchaseUnits, // Lista de purchase_units
                "USD", // Moneda
                "http://localhost:4200/cliente/payment/success", // URL éxito
                "http://localhost:4200/cliente/payment/cancel"   // URL cancelación
            );

            var approvalUrl = order.links.FirstOrDefault(link => link.rel == "approve")?.href;
            if (approvalUrl == null)
            {
                return BadRequest("No se pudo obtener la URL de aprobación.");
            }

            var orderID = order.id;

            PagoPayPal nuevoPago = new PagoPayPal
            {
                linkPago = approvalUrl,
                pagoId = orderID
            };

            try
            {
                _blAdministrativo.AddPaypalPago(nuevoPago);
                var pagoCreado = _blAdministrativo.GetPaypalPagoByOrdenId(orderID);
                return Ok(pagoCreado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("capture/{orderId}")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        try
        {
            var capture = await _payPalService.CaptureOrderAsync(orderId);
            return Ok(capture);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("details/{orderId}")]
    public async Task<IActionResult> GetOrderDetails(string orderId)
    {
        try
        {
            var orderDetails = await _payPalService.GetOrderDetailsAsync(orderId);
            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class PaymentRequest
{
    public List<PayPalPurchaseUnit> PurchaseUnits { get; set; }
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