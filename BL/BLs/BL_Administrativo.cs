using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Globalization;
using PayPal.Api;

namespace BL.BLs
{
	public class BL_Administrativo : IBL_Administrativo
	{
		private readonly IDAL_Administrativo dal;
		private readonly IDAL_Pacientes dal_Paciente;
		private readonly ILogger<BL_Administrativo> _logger;
		private readonly IChannel channel;

		public BL_Administrativo(IDAL_Administrativo dal, ILogger<BL_Administrativo> logger, IDAL_Pacientes dal_Paciente, IChannel channel)
		{
			_logger = logger;
			this.dal = dal;
			this.dal_Paciente = dal_Paciente;
			this.channel = channel;
		}

		//Pacientes 
		#region PACIENTES

		public void addPaciente(Paciente paciente)
		{
			dal.AddPaciente(paciente);
		}

		public void deletePaciente(long id)
		{
			dal.DeletePaciente(id);
		}

		public Paciente getPacienteById(long id)
		{
			return dal.GetPacienteById(id);
		}

		public Paciente getPacienteByDNI(string dni)
		{
			return dal.GetPacienteByDNI(dni);
		}

		public List<Paciente> getPacientes()
		{
			return dal.GetPacientes();
		}

		public void updatePaciente(Paciente paciente)
		{
			dal.UpdatePaciente(paciente);
		}

		public List<Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro)
		{
			return dal.GetPacientesFiltradosPaginados(numPagina, filtro);
		}

		public bool emailDuplicado(string email)
		{
			return dal.emailDuplicado(email);
		}

		public bool cedulaDuplicada(string cedula)
		{
			return dal.cedulaDuplicada(cedula);
		}

		public List<Notificacion> getNotificaciones(long id, int pageNumber, int pageSize)
		{
			return dal.getNotificaciones(id, pageNumber, pageSize);
		}

		public int CountNotificaciones(long id)
		{
			return dal.CountNotificaciones(id);
		}

		public List<Factura> getHistorialFacturacion(long id, int pageNumber, int pageSize)
		{
			return dal.getHistorialFacturacion(id, pageNumber, pageSize);
		}

		public int countFacturas(long id)
		{
			return dal.countFacturas(id);
		}

        #endregion

		//Seguros Medicos
		#region SEGUROS MEDICOS

		public void addSeguroMedico(SeguroMedico seguroMedico)
		{
			dal.AddSeguroMedico(seguroMedico);
		}

		public void deleteSeguroMedico(long id)
		{
			dal.DeleteSeguroMedico(id);
		}

		public SeguroMedico getSeguroMedicoById(long id)
		{
			return dal.GetSeguroMedicoById(id);
		}

		public List<SeguroMedico> getSegurosMedicos()
		{
			return dal.GetSegurosMedicos();
		}

		public void updateSeguroMedico(SeguroMedico seguroMedico)
		{
			dal.UpdateSeguroMedico(seguroMedico);
		}

		#endregion

		//Contratos
		#region CONTRATOS

		public List<Contrato> getContratos()
		{
			return dal.GetContratos();
		}

		public Contrato getContratoById(long id)
		{
			return dal.GetContratoById(id);
		}

		public void addContrato(Contrato contrato)
		{
			dal.AddContrato(contrato);
		}

		public void updateContrato(Contrato contrato)
		{
			dal.UpdateContrato(contrato);
		}

		public void deleteContrato(long id)
		{
			dal.DeleteContrato(id);
		}

        public void cambiarContrato(Contrato contrato, SeguroMedico seguroMedico)
		{
			contrato.SeguroMedico = seguroMedico;
			contrato.FechaInicio = DateTime.UtcNow;
			updateContrato(contrato);
        }

        public void ContratarSeguroMedico(long idPaciente, long idSeguroMedico)
		{
			var paciente = getPacienteById(idPaciente);
			var seguroMedico = getSeguroMedicoById(idSeguroMedico);
			if (paciente != null && seguroMedico != null)
			{

				// Verificar si el paciente ya tiene un contrato asociado
				if (paciente.Contrato != null)
				{
					throw new InvalidOperationException("El paciente ya tiene un contrato existente.");
				}

				Contrato contrato = new Contrato()
				{
					Paciente = paciente,
					SeguroMedico = seguroMedico,
					FechaInicio = DateTime.Now,
					Activo = false,
				};
				addContrato(contrato);

				paciente.Contrato = contrato;
				updatePaciente(paciente);

				seguroMedico.Contratos.Add(contrato);
				updateSeguroMedico(seguroMedico);
			}
		}

        public bool contratoEnRefinanciacion(long contratoId)
		{
			return dal.contratoEnRefinanciacion(contratoId);
        }

        public bool puedeRenovarContrato(long id)
		{
			return dal.puedeRenovarContrato(id);
		}

        public void activarContrato(long id)
        {
            var contrato = getContratoById(id);
            if (contrato != null)
            {
                contrato.Activo = true;
                updateContrato(contrato);
            }
        }

        public List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro)
		{
			return dal.GetContratosFiltradosPaginados(numPagina, filtro);
		}

		public List<Factura> ObtenerUltimasFacturasDelContrato(long contratoId, int cantidad)
		{
			return dal.ObtenerUltimasFacturasDelContrato(contratoId, cantidad);
		}

        public float ObtenerDeudaDeContrato(long contratoId)
		{
			return dal.ObtenerDeudaDeContrato(contratoId);
        }

		public void reactivarContrato(long contratoId, int cantidadCuotas, int interes)
		{
			var contrato = getContratoById(contratoId);
			var deuda = ObtenerDeudaDeContrato(contratoId);
			if (deuda > 0)
			{
                var montoTotalConInteres = deuda * (1 + (interes / 100.0));
                var cuota = (float)(montoTotalConInteres / cantidadCuotas);

                // Crear las facturas
                for (int i = 0; i < cantidadCuotas; i++)
				{
					var factura = new Factura
					{
						Fecha = DateTime.Now.AddMonths(i + 1),
						Monto = cuota,
						Pago = false,
						Descripcion = $"Cuota {i + 1} de {cantidadCuotas} de la deuda del contrato",
						Paciente = contrato.Paciente,
						FechaPago = null
					};
					addFactura(factura);
				}

			}
			contrato.Activo = true;
			contrato.FechaInicio = DateTime.UtcNow;
			updateContrato(contrato);
        }

		#endregion

		//Precios
		#region PRECIOS


		public List<Precio> getPrecios()
		{
			return dal.GetPrecios();
		}

		public Precio getPrecioById(long id)
		{
			return dal.GetPrecioById(id);
		}

		public Precio GetPrecioBySeguro(long id)
        {
            return dal.GetPrecioBySeguro(id);
        }

        public void addPrecio(Precio precio)
		{
			dal.AddPrecio(precio);
		}

		public void updatePrecio(Precio precio)
		{
			dal.UpdatePrecio(precio);
		}

		public void deletePrecio(long id)
		{
			dal.DeletePrecio(id);
		}

		#endregion

		//Copagos
		#region COPAGOS

		public List<Copago> getCopagos()
		{
			return dal.GetCopagos();
		}

		public Copago getCopagoById(long id)
		{
			return dal.GetCopagoById(id);
		}

		public void addCopago(Copago copago)
		{
			dal.AddCopago(copago);

			//si el copago incluye un precio, se agrega a la lista de precios
			if (copago.Precios != null && copago.Precios.Count > 0)
			{
				var copagoId = dal.getIdByFilds(copago);
				_logger.LogInformation("CopagoId: " + copagoId);
				foreach (var precio in copago.Precios)
				{
					precio.Copago.Id = copagoId;
					dal.AddPrecio(precio);
				}
			}
		}

		public long getIdByFilds(Copago copago)
        {
           return dal.getIdByFilds(copago);
        }

        public void updateCopago(Copago copago)
		{
			dal.UpdateCopago(copago);
		}

		public void deleteCopago(long id)
		{
			//borrar precios asociados al copago
			var precios = getPrecios();
			var preciosAEliminar = precios.Where(p => p.Copago != null && p.Copago.Id == id).ToList();

			_logger.LogInformation("Precios a borrar: " + preciosAEliminar.Count);
			foreach (var precio in preciosAEliminar)
			{
				deletePrecio(precio.Id);
			}
			_logger.LogInformation("Precios borrados");

			dal.DeleteCopago(id);
		}

		#endregion

		//Facturas
		#region FACTURAS

		public List<Factura> getFacturas()
		{
			return dal.GetFacturas();
		}

        public List<Factura> GetFacturasByPaypal(string paypalOrderId)
        {
            return dal.GetFacturasByPaypal(paypalOrderId);
        }


        public List<Factura> getFacturasPaginadas(int numPagina, string? pacienteString, bool fechaAsc, bool? estaPago)
		{
			return dal.GetFacturasPaginadas(numPagina, pacienteString, fechaAsc, estaPago);
		}

		public Factura getFacturaById(long id)
		{
			return dal.GetFacturaById(id);
		}

		public void addFactura(Factura factura)
		{
			dal.AddFactura(factura);
		}

		public void updateFactura(Factura factura)
		{
			dal.UpdateFactura(factura);
		}

		public void deleteFactura(long id)
		{
			dal.DeleteFactura(id);
		}

		public async Task GenerarFacturasAutomaticas()
		{
			// Obtén todos los contratos activos
			var contratosActivos = dal.GetContratosActivos();

			foreach (var contrato in contratosActivos)
			{
				Paciente paciente = dal.GetPacienteById(contrato.Paciente.Id);

                // Verifica si ya existe una factura para el paciente en el mes actual
                bool facturaExistente = dal.ExisteFacturaParaPacienteEnMes(paciente.Id, DateTime.Now.Month, DateTime.Now.Year);

                if (!facturaExistente)
                {
                    // Verifica si ya existen dos facturas sin pagar entre las tres últimas facturas
                    var ultimasFacturas = dal.ObtenerUltimasFacturasDelContrato(contrato.Id, 3);
                    int facturasNoPagadas = ultimasFacturas.Count(f => !f.Pago);

                    if (facturasNoPagadas >= 2)
                    {
                        Console.WriteLine($"El contrato {contrato.Id} tiene dos facturas sin pagar. Generando la tercera factura y desactivando el contrato.");
                        // Desactiva el contrato
                        contrato.Activo = false;
                        dal.UpdateContrato(contrato);
                    }

                    // Crea una nueva factura para cada contrato
                    var factura = new Factura
                    {
                        Fecha = DateTime.Now,
                        Monto = ObtenerMontoFactura(contrato), // Puedes definir este método para obtener el monto del seguro
                        Descripcion = $"Mensualidad de seguro médico: {contrato.SeguroMedico.Nombre}",
                        FechaPago = null,
                        Pago = false,
                        Paciente = paciente
                    };

                    // Buscar facturas anteriores al mes actual
                    var facturasPendientes = new List<Factura>();
                    var facturaMesAnterior = dal.ObtenerFacturaParaPacienteEnMes(paciente.Id, DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year);

                    if (facturaMesAnterior != null)
                    {
                        // Si hay una factura del mes anterior, busca facturas entre ambas fechas
                        facturasPendientes = dal.ObtenerFacturasEnRangoFechas(paciente.Id, facturaMesAnterior.Fecha, factura.Fecha)
                            .Where(f => !f.Pago).ToList();
                    }
                    else
                    {
                        // Si no hay factura del mes anterior, busca todas las facturas pendientes
                        facturasPendientes = dal.ObtenerFacturasNoPagadasParaPaciente(paciente.Id);
                    }

                    // Guarda la factura en la base de datos
                    //dal.AddFactura(factura);

                    try
                    {
                        // Crear orden de PayPal
                        // Crear unidades de compra (PayPalPurchaseUnit) para todas las facturas pendientes y la factura actual
                        var purchaseUnits = facturasPendientes
                            .Select(f => new PayPalPurchaseUnit
                            {
                                reference_id = f.Id.ToString(),
                                amount = new PayPalAmount
                                {
                                    currency_code = "USD",
                                    value = f.Monto.ToString("F2", CultureInfo.InvariantCulture)
                                },
                                description = f.Descripcion
                            })
                            .ToList();

                        // Añadir la factura actual como una unidad de compra
                        purchaseUnits.Add(new PayPalPurchaseUnit
                        {
                            reference_id = factura.Id.ToString(),
                            amount = new PayPalAmount
                            {
                                currency_code = "USD",
                                value = factura.Monto.ToString("F2", CultureInfo.InvariantCulture)
                            },
                            description = factura.Descripcion
                        });

                        var orderResponse = await dal.CreateOrderAsync(purchaseUnits,
							"USD",
                            "https://localhost:4200/cliente/payment/success",
                            "https://localhost:4200/cliente/payment/cancel");

                        // Guarda la orden de PayPal en la base de datos
                        var nuevoPago = new PagoPayPal
                        {
                            linkPago = orderResponse.links.FirstOrDefault(link => link.rel == "approve")?.href,
                            pagoId = orderResponse.id
                        };

                        dal.AddPaypalPago(nuevoPago);
                        var pagoCreado = dal.GetPaypalPagoByOrdenId(orderResponse.id);

                        var factura2 = new Factura
                        {
                            Fecha = DateTime.Now,
                            Monto = ObtenerMontoFactura(contrato), // Puedes definir este método para obtener el monto del seguro
                            Descripcion = $"Mensualidad de seguro médico: {contrato.SeguroMedico.Nombre}",
                            FechaPago = null,
                            Pago = false,
                            Paciente = paciente,
							PagoPayPal = pagoCreado
                        };
                        dal.AddFactura(factura2);

                        foreach (var facturaPendiente in facturasPendientes)
                        {
                            facturaPendiente.PagoPayPal = pagoCreado;
                            dal.UpdateFactura(facturaPendiente);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al crear la orden de PayPal para la factura {factura.Id}: {ex.Message}");
                    }

                    var notificacion = new Notificacion
                    {
                        Mensaje = $"Tiene una nueva factura pendiente de pago para la mensualidad de su seguro médico: {contrato.SeguroMedico.Nombre}.",
                        FechaEnvio = DateTime.UtcNow,
                        Visto = false
                    };

                    // Puedes agregar la notificación en el sistema del paciente aquí
                    // dal_Paciente.AddNotificacion(notificacion, paciente.Id);
                }
                else
                {
                    Console.WriteLine($"La factura para el contrato {contrato.Id} ya fue emitida este mes.");
                }
            }

            await dal.SaveChangesAsync();
        }


        private float ObtenerMontoFactura(Contrato contrato)
        {
			// Aquí puedes definir la lógica para calcular el monto de la factura basado en el contrato
			Precio preciobtenido = dal.GetPrecioBySeguro(contrato.SeguroMedico.Id);
			float numero = preciobtenido.PrecioBase;
			return numero;
		}

		public MemoryStream GenerarFactura(long id)
		{
			Factura factura = dal.GetFacturaById((int)id);

			var memoryStream = new MemoryStream();
			Document document = new Document(PageSize.A4, 25, 25, 30, 30);
			PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
			writer.CloseStream = false;

			document.Open();

			// Encabezado de la factura
			var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
			var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
			var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

			document.Add(new Paragraph("FACTURA", titleFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("ASOCIACIÓN MÉDICA SAN JOSÉ", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("Treinta y Tres 633", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("RUT 170114500018", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("\n"));

			// Información del cliente
			document.Add(new Paragraph("Información del Cliente", boldFont));
			document.Add(new Paragraph($"Nombre: {factura.Paciente.Nombres} {factura.Paciente.Apellidos}", regularFont));
			document.Add(new Paragraph($"Documento: {factura.Paciente.Documento}", regularFont));
			document.Add(new Paragraph("\n"));

			// Información de la factura
			document.Add(new Paragraph("Detalles de la Factura", boldFont));
			document.Add(new Paragraph($"Fecha de Emisión: {factura.Fecha:dd/MM/yyyy}", regularFont));
			document.Add(new Paragraph("\n"));

			// Tabla de detalles (solo ejemplo con un concepto y el monto)
			PdfPTable table = new PdfPTable(2);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 3, 1 }); // Ancho de columnas: Descripción, Monto

			// Encabezado de la tabla
			PdfPCell cell = new PdfPCell(new Phrase("Descripción", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			cell.BackgroundColor = new BaseColor(230, 230, 230);
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("Monto", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			cell.BackgroundColor = new BaseColor(230, 230, 230);
			table.AddCell(cell);

			// Detalle de la factura
			table.AddCell(new PdfPCell(new Phrase($"{factura.Descripcion}", regularFont)));
			table.AddCell(new PdfPCell(new Phrase($"{factura.Monto.ToString("C", new CultureInfo("en-US"))}", regularFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });

			// Total
			cell = new PdfPCell(new Phrase("Total", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_RIGHT;
			cell.Colspan = 1;
			cell.Border = Rectangle.TOP_BORDER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase($"{factura.Monto.ToString("C", new CultureInfo("en-US"))}", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_RIGHT;
			cell.Border = Rectangle.TOP_BORDER;
			table.AddCell(cell);

			document.Add(table);

			document.Close();
			memoryStream.Position = 0;

			return memoryStream;
		}

		public MemoryStream GenerarFacturaListada(List<long> ids)
		{
			var memoryStream = new MemoryStream();
			Document document = new Document(PageSize.A4, 25, 25, 30, 30);
			PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
			writer.CloseStream = false;

			document.Open();

			// Estilos de fuente
			var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
			var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
			var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

			// Encabezado del documento
			document.Add(new Paragraph("FACTURAS", titleFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("ASOCIACIÓN MÉDICA SAN JOSÉ", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("Treinta y Tres 633", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("RUT 170114500018", regularFont) { Alignment = Element.ALIGN_CENTER });
			document.Add(new Paragraph("\n"));

			// Inicializar tabla
			PdfPTable table = new PdfPTable(2);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 3, 1 }); // Ancho de columnas: Descripción, Monto

			// Encabezado de la tabla
			PdfPCell cell = new PdfPCell(new Phrase("Descripción", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			cell.BackgroundColor = new BaseColor(230, 230, 230);
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("Monto", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			cell.BackgroundColor = new BaseColor(230, 230, 230);
			table.AddCell(cell);

			// Acumular monto total
			decimal montoTotal = 0;

			// Recorrer los IDs y agregar cada factura
			foreach (var id in ids)
			{
				Factura factura = dal.GetFacturaById((int)id);

				if (factura != null)
				{
					table.AddCell(new PdfPCell(new Phrase($"{factura.Descripcion}", regularFont)));
					table.AddCell(new PdfPCell(new Phrase($"{factura.Monto.ToString("C", new CultureInfo("en-US"))}", regularFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });

					montoTotal += (decimal)factura.Monto;
				}
			}

			// Total de las facturas
			cell = new PdfPCell(new Phrase("Total", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_RIGHT;
			cell.Border = Rectangle.TOP_BORDER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase($"{montoTotal.ToString("C", new CultureInfo("en-US"))}", boldFont));
			cell.HorizontalAlignment = Element.ALIGN_RIGHT;
			cell.Border = Rectangle.TOP_BORDER;
			table.AddCell(cell);

			document.Add(table);

			document.Close();
			memoryStream.Position = 0;

			return memoryStream;
		}

		#endregion

		//Medicos
		#region MEDICOS

		public List<Medico> getMedicos()
		{
			return dal.GetMedicos();
		}

		public Medico getMedicoById(long id)
		{
			return dal.GetMedicoById(id);
		}

		public Medico getMedicoByDocumento(string documento)
		{
			return dal.GetMedicoByDocumento(documento);
		}

		public void addMedico(Medico medico)
		{
			dal.AddMedico(medico);
		}

		public void updateMedico(Medico medico)
		{
			dal.UpdateMedico(medico);
		}

		public void deleteMedico(long id)
		{
			//desactivar los calendarios del medico
			var calendarios = getCalendarios().Where(c => c.Medico.Id == id).ToList();
			foreach (var calendario in calendarios)
			{
				deleteCalendario(calendario.Id);
			}

			dal.DeleteMedico(id);
		}

		public void asignarEspecialidad(long medId, long espId)
		{
			var medico = dal.GetMedicoById(medId);
			var especialidad = dal.GetEspecialidadById(espId);

			if (medico != null && especialidad != null)
			{
				medico.Especialidades.Add(especialidad);
				dal.UpdateMedico(medico);
			}
		}

		public List<Medico> getMedicosPaginadosYFiltrados(int numPagina, string filtro)
		{
			return dal.GetMedicosPaginadosYFiltrados(numPagina, filtro);
		}


		#endregion

		//Citas Medicas
		#region CITAS MEDICAS

		public List<CitaMedica> getCitasMedicas()
		{
			return dal.GetCitasMedicas();
		}

		public CitaMedica getCitaMedicaById(long id)
		{
			return dal.GetCitasMedicasById(id);
		}

		public void addCitaMedica(CitaMedica citaMedica)
		{
			dal.AddCitasMedicas(citaMedica);
		}

		public void updateCitaMedica(CitaMedica citaMedica)
		{
			dal.UpdateCitasMedicas(citaMedica);
		}

		public void deleteCitaMedica(long id)
		{
			dal.DeleteCitasMedicas(id);
		}

		#endregion

		//Calendarios
		#region CALENDARIOS

		public List<Calendario> getCalendarios()
		{
			return dal.GetCalendarios();
		}

		public Calendario getCalendarioById(long id)
		{
			return dal.GetCalendarioById(id);
		}

		public void addCalendario(Calendario calendario)
		{
			dal.AddCalendario(calendario);
		}

		public void updateCalendario(Calendario calendario)
		{
			dal.UpdateCalendario(calendario);
		}

		public async void deleteCalendario(long calendarioId)
		{
			//borrar el calendario y cancelar las citas asociadas
			var citas = getCitasMedicas()
				.Where(c => c.Calendario.Id == calendarioId)
				.Where(c => c.Estado == "Agendada")
				.ToList();
			Console.WriteLine("cantidad de citas a borrar" + citas.Count);
			foreach (var cita in citas)
			{
				cita.Estado = "Cancelada";
				updateCitaMedica(cita);

				//Creo la notificacion para el paciente
				if (cita.PacienteId != null)
				{
					var notificacion = new Notificacion()
					{
						Mensaje = $"Su Cita medica para la fecha {cita.Fecha} ha tenido que ser cancelada, por favor agende nuevamente",
						FechaEnvio = DateTime.Now,
						Visto = false
					};
					string id = (string)cita.PacienteId;
					string IdDesencriptada = AES.Decrypt(id);
					notificacion.Paciente.Id = long.Parse(IdDesencriptada);
					//dal_Paciente.AddNotificacion(notificacion, id);


					//uso la coneccion a rabbimq para enviar la notificacion a la cola
					try
					{
						var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notificacion));

						//envio la notificacion por rabbit
						await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "Notificaciones", body: body);
					}
					catch (Exception ex)
					{
						_logger.LogError($"Error al enviar notificación: {ex.Message}");
						throw;
					}

				}
			}

			_logger.LogInformation("Se eliminaron las ciras");

			//se desactiva el calendario
			dal.DeleteCalendario(calendarioId);
		}

		public void crearCalendario(long medId, long espId, long conId, TimeSpan horaInicio, TimeSpan horaFin, int tiempo, int cant, string[] dias)
		{
			var medico = getMedicoById(medId);
			var especialidad = getEspecialidadById(espId);
			var consultorio = getConsultorioById(conId);
			if (medico != null && especialidad != null && consultorio != null)
			{

				//verificar que la especialidad sea una de las del medico
				if (!medico.Especialidades.Any(e => e.Id == espId))
				{
					throw new InvalidOperationException("La especialidad no está asociada con el médico.");
				}

				Calendario calendario = new Calendario()
				{
					Medico = medico,
					Especialidad = especialidad,
					Consultorio = consultorio,
					HoraInicio = horaInicio,
					HoraFin = horaFin,
					TiempoCita = tiempo,
					CantidadCitas = cant,
					DiasSemana = dias
				};
				addCalendario(calendario);
				return;
			}
			else
			{
				throw new InvalidOperationException("No se pudo crear el calendario.");
			}
		}

		public bool checkOcupacionConsultorio(Calendario calendario)
		{
			var calendarios = getCalendarios();
			if(calendarios == null || calendarios.Count == 0)
			{
				return false;
			}

			//verificar que no haya otro calendario que se cruce con el nuevo
			foreach (var c in calendarios)
			{
				if(c.Id == calendario.Id)
				{
					continue;
				}
				if (c.Consultorio.Id == calendario.Consultorio.Id)
				{
					if (c.DiasSemana.Intersect(calendario.DiasSemana).Any())
					{
						_logger.LogInformation("conflicto con calendario en los dias " + string.Join(",", c.DiasSemana) + " para los dias " + string.Join(",", calendario.DiasSemana));
						if ((c.HoraInicio < calendario.HoraInicio && c.HoraFin > calendario.HoraInicio) ||
							(c.HoraInicio < calendario.HoraFin && c.HoraFin > calendario.HoraFin) ||
							(c.HoraInicio >= calendario.HoraInicio && c.HoraFin <= calendario.HoraFin) ||
							(c.HoraInicio <= calendario.HoraInicio && c.HoraFin >= calendario.HoraFin))
						{
							_logger.LogInformation("conflicto con calendario en las horas " + c.HoraInicio + " - " + c.HoraFin + " para la hora de inicio " + calendario.HoraInicio + " y hora de fin " + calendario.HoraFin);
							return true;
						}
					}
				}
			}
			_logger.LogInformation("saliendo sin conflictos");
			return false;
		}

		public bool validarCalendariosPropios(long medicoId, long calendarioEditId, Calendario calendario)
		{
			//chequear que ningun calendario del medico se cruce en horario con el nuevo calendario
			var calendarios = getCalendarios().Where(c => c.Medico.Id == medicoId).ToList();
			if (calendarios.Count == 0 || calendarios == null)
			{
				return true;
			}

			foreach (var c in calendarios)
			{
				if (calendarioEditId != 0 && c.Id == calendarioEditId)
				{
                    //si es el calendario que se esta editando, se saltea para evitar conflictos consigo mismo
                    continue;
                }
                //chequeo por cualquier dia del calendario c que este en el calendario nuevo
                if (c.DiasSemana.Intersect(calendario.DiasSemana).Any())
                {
					Console.WriteLine("Dia nuevo: " + string.Join(",", c.DiasSemana.Intersect(calendario.DiasSemana)));
                    if ((c.HoraInicio < calendario.HoraInicio && c.HoraFin > calendario.HoraInicio) ||
                        (c.HoraInicio < calendario.HoraFin && c.HoraFin > calendario.HoraFin) ||
                        (c.HoraInicio >= calendario.HoraInicio && c.HoraFin <= calendario.HoraFin) ||
                        (c.HoraInicio <= calendario.HoraInicio && c.HoraFin >= calendario.HoraFin))
                    {
                        Console.WriteLine(c.HoraInicio + " - " + c.HoraFin + " - " + calendario.HoraInicio + " - " + calendario.HoraFin);
                        return false;
                    }
                }
            }

			//salio sin conflictos
			return true;
		}

		public bool validarEspecialidadesParaBorrar(long medicoId, List<Especialidad> especialidades)
		{
			//validar que de los calendarios del medico, ninguno sea para una especialidad que no este en la lista
			var calendarios = getCalendarios().Where(c => c.Medico.Id == medicoId).ToList();
			if (calendarios.Count == 0 || calendarios == null)
			{
				return true;
			}

			foreach (var calendario in calendarios)
			{
				if(!especialidades.Any(e => e.Id == calendario.Especialidad.Id)){
					_logger.LogInformation("Especialidad no encontrada en la lista de especialidades a borrar");
					return false;
				}

			}

			return true;
		}

		public async Task borrarCalendariosIncompatiblesAsync(long medicoId, List<Especialidad> especialidades)
		{
			//eliminar aquellos calendarios de este medico cuya especialidad no este en la lsita de especialidades
			var calendarios = getCalendarios().Where(c => c.Medico.Id == medicoId).ToList();
			foreach (var calendario in calendarios)
			{
				if (!especialidades.Any(e => e.Id == calendario.Especialidad.Id))
				{
					//se desactiva el calendario
					deleteCalendario(calendario.Id);
				}
			}
		}

		public List<Calendario> getCalendariosFiltrados(long medicoId, string filtroEspecialidad, string filtroDia, string filtroHoraInicio)
		{
			return dal.GetCalendariosFiltrados(medicoId, filtroEspecialidad, filtroDia, filtroHoraInicio);

		}

		public List<Calendario> getCalendariosByArticuloFecha(string cedula, long articuloId, string fecha)
		{
            //segun la fecha conseguir el dia
            DateTime fechaDate = DateTime.Parse(fecha);
            string dia = fechaDate.ToString("dddd", new CultureInfo("es-UY"));
            //poner la primera letra en mayuscula 
            dia = char.ToUpper(dia[0]) + dia.Substring(1);

            Paciente paciente = getPacienteByDNI(cedula);
            if (paciente == null)
            {
                return null;
            }

            SeguroMedico seguro = paciente.Contrato.SeguroMedico;
            if (seguro == null)
            {
                return null;
            }

			List<Especialidad> especialidadesParaElArticulo = dal.GetEspecialidadesByArticuloSeguro(articuloId, seguro.Id);

			List<Calendario> calendariosParaELArticulo = new List<Calendario>();

			foreach(var especialidad in especialidadesParaElArticulo)
			{
				//ir agregando los calendarios a la lista
                var calendarios = dal.GetCalendariosByEspecialidadFecha(especialidad.Id, fechaDate, dia);
                calendariosParaELArticulo.AddRange(calendarios);
			}

			return calendariosParaELArticulo;
        }

        #endregion

        //Consultorios
        #region CONSULTORIOS


        public List<Consultorio> getConsultorios()
		{
			return dal.GetConsultorios();
		}

		public Consultorio getConsultorioById(long id)
		{
			return dal.GetConsultorioById(id);
		}

		public void addConsultorio(Consultorio consultorio)
		{
			dal.AddConsultorio(consultorio);
		}

		public void updateConsultorio(Consultorio consultorio)
		{
			dal.UpdateConsultorio(consultorio);
		}

		public void deleteConsultorio(long id)
		{
			dal.DeleteConsultorio(id);
		}

		#endregion

		//Especialidades
		#region ESPECIALIDADES

		public List<Especialidad> getEspecialidades()
		{
			return dal.GetEspecialidades();
		}

		public Especialidad getEspecialidadById(long id)
		{
			return dal.GetEspecialidadById(id);
		}

		public void addEspecialidad(Especialidad especialidad)
		{
			dal.AddEspecialidad(especialidad);
		}

		public void updateEspecialidad(Especialidad especialidad)
		{
			dal.UpdateEspecialidad(especialidad);
		}

		public void deleteEspecialidad(long id)
		{
			dal.DeleteEspecialidad(id);
		}

		#endregion

		//Articulos
		#region ARTICULOS

		public List<Articulo> getArticulos()
		{
			return dal.GetArticulos();	
		}

		public Articulo getArticuloById(long id)
		{
			return dal.GetArticuloById(id);
		}

		public void addArticulo(Articulo articulo)
		{
			dal.AddArticulo(articulo);
		}

		public void updateArticulo(Articulo articulo)
		{
			dal.UpdateArticulo(articulo);
		}

		public void deleteArticulo(long id)
		{
			dal.DeleteArticulo(id);
		}

		public List<Articulo> getArticulosFiltrados(string filtro)
		{
			return dal.GetArticulosFiltrados(filtro);
		}

        public List<Articulo> getArticulosHabilitados(string cedula)
		{
			Paciente paciente = getPacienteByDNI(cedula);
            if (paciente == null)
			{
				return null;
			}

			Contrato contrato = paciente.Contrato;
			if(contrato == null || !contrato.Activo)
            {
                return null;
            }

            SeguroMedico seguro = contrato.SeguroMedico;
            if (seguro == null)
			{
				return null;
			}

            return dal.GetArticulosBySeguro(seguro);
		}

        #endregion

        //Pago PayPal
        #region PAGO PAYPAL
        public List<PagoPayPal> GetPaypalPagos()
		{
			return dal.GetPaypalPagos();
		}

		public PagoPayPal GetPaypalPagoById(long id)
		{
			return dal.GetPaypalPagoById(id);
		}

        public PagoPayPal GetPaypalPagoByOrdenId(string id)
        {
            return dal.GetPaypalPagoByOrdenId(id);
        }

        public void AddPaypalPago(PagoPayPal nuevoPago)
        {
            dal.AddPaypalPago(nuevoPago);
        }

		#endregion
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
}
