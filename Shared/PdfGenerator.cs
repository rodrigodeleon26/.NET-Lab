using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Shared
{
    public class PdfGenerator
    {
        public byte[] GeneratePdf(EstudioDTO datosEstudio, Paciente paciente)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Crear un documento PDF
                var document = new Document();
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Agregar título al PDF
                var titulo = new Paragraph("Estudio", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER; // Alinear al centro
                document.Add(titulo);

                // Agregar un espacio después del título
                document.Add(new Paragraph("\n"));

                var subtituloPaciente = new Paragraph("Paciente", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                subtituloPaciente.Alignment = Element.ALIGN_LEFT; // Alinear a la izquierda
                document.Add(subtituloPaciente);

                // Agregar un espacio después del subtítulo
                document.Add(new Paragraph("\n"));

                // Agregar información del paciente
                document.Add(new Paragraph("Cédula: " + paciente.Documento));
                document.Add(new Paragraph("Nombre: " + paciente.Nombres + " " + paciente.Apellidos));
                DateOnly fecha = paciente.FechaDeNacimiento ?? new DateOnly();
                document.Add(new Paragraph("Fecha de nacimiento: " + fecha.ToString("dd/MM/yyyy")));

                document.Add(new Paragraph("\n"));

                var subtituloEstudio = new Paragraph("Informacion del estudio", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                subtituloEstudio.Alignment = Element.ALIGN_LEFT; // Alinear a la izquierda
                document.Add(subtituloEstudio);

                // Agregar un espacio después del subtítulo
                document.Add(new Paragraph("\n"));

                // Agregar contenido al PDF
                document.Add(new Paragraph("Nombre: " + datosEstudio.Nombre));
                document.Add(new Paragraph("Descripción: " + datosEstudio.Descripcion));
                document.Add(new Paragraph("Fecha: " + datosEstudio.FechaRealizado.ToString("dd/MM/yyyy")));
                document.Add(new Paragraph("Fecha de resultado: " + datosEstudio.FechaResultado.ToString("dd/MM/yyyy")));
                document.Add(new Paragraph("Resultado: Un éxito"));
               

                // Cerrar el documento
                document.Close();

                // Devolver el PDF como un array de bytes
                return memoryStream.ToArray();
            }
        }
    }

}
