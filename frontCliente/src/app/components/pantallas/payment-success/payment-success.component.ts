import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaypalService } from '../../../services/paypal.service';
import { FacturaService } from '../../../services/factura.service';

@Component({
  selector: 'app-payment-success',
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.css'
})
export class PaymentSuccessComponent {
  statusMessage: string = '';

  constructor(
    private paypalService: PaypalService,
    private facturaService: FacturaService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Captura los parámetros de la URL
    this.route.queryParams.subscribe((params) => {
      const orderId = params['token']; // `token` contiene el ID de la orden en PayPal
      const payerId = params['PayerID']; // Si necesitas más datos (como el ID del pagador)
  
      if (orderId) {
        // 1. Capturar el pago
        this.paypalService.capturePayment(orderId).subscribe(
          (response) => {
            console.log('Pago capturado exitosamente:', response);
            this.statusMessage = '¡Pago realizado con éxito!';
            
            // 2. Traer todas las facturas asociadas al orderId
            this.facturaService.getFacturasByPaypal(orderId).subscribe(
              (facturas) => {
                console.log('Facturas asociadas:', facturas);
  
                // 3. Iterar sobre las facturas y marcarlas como pagadas
                facturas.forEach((factura) => {
                  factura.pago = true; // Marcar como pagado
                  factura.fechaPago = new Date().toISOString(); // Establecer la fecha de pago
  
                  // Llamar al método para actualizar la factura en el backend
                  this.facturaService.updateFactura(factura.id, factura).subscribe(
                    (updateResponse) => {
                      console.log(`Factura ${factura.id} actualizada correctamente.`, updateResponse);
                    },
                    (updateError) => {
                      console.error(`Error al actualizar la factura ${factura.id}:`, updateError);
                    }
                  );
                });
              },
              (error) => {
                console.error('Error al obtener facturas asociadas al orderId:', error);
              }
            );
          },
          (error) => {
            console.error('Error al capturar el pago:', error);
            this.statusMessage = 'Hubo un problema al capturar el pago.';
          }
        );
      } else {
        this.statusMessage = 'No se encontró un identificador de orden válido.';
      }
    });
  }
}
