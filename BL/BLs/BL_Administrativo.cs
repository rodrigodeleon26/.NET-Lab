using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using DAL.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace BL.BLs
{
	public class BL_Administrativo : IBL_Administrativo
	{
		private readonly IDAL_Administrativo dal;
        private readonly ILogger<BL_Administrativo> _logger;

        public BL_Administrativo(IDAL_Administrativo dal, ILogger<BL_Administrativo> logger)
		{
            _logger = logger;
			this.dal = dal;
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

		public void activarContrato(long id)
		{
			var contrato = getContratoById(id);
			if (contrato != null)
			{
				contrato.Activo = true;
				updateContrato(contrato);
			}
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
                Paciente paciente = dal.GetPacienteById(contrato.Id);
                // Crea una nueva factura para cada contrato
                var factura = new Factura
                {
                    Fecha = DateTime.Now,
                    Monto = ObtenerMontoFactura(contrato), // Puedes definir este método para obtener el monto del seguro
                    Descripcion = $"Pago de {contrato.SeguroMedico.Nombre}",
                    FechaPago = null,
                    Pago = false,
                    Paciente = paciente
                };

                // Guarda la factura en la base de datos
                dal.AddFactura(factura);
            }

            await dal.SaveChangesAsync(); // Guarda todos los cambios en la base de datos
        }

        private float ObtenerMontoFactura(Contrato contrato)
        {
			// Aquí puedes definir la lógica para calcular el monto de la factura basado en el contrato
			float numero = 200;
            return numero; // Este es solo un ejemplo
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

		public void deleteCalendario(long id)
		{
			dal.DeleteCalendario(id);
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

		#endregion
	}
}
