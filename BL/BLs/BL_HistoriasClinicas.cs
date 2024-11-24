using Shared;
using BL.IBLs;
using DAL.IDALs;
using DAL.Models;
using Shared.Services;
using iTextSharp.text.pdf;

namespace BL.BLs
{
    public class BL_HistoriasClinicas : IBL_HistoriasClinicas
    {
        private readonly IDAL_HistoriasClinicas dal;
        private readonly IDAL_Administrativo dalAdminService;
        private readonly IDAL_CitasMedicas dalCitasMedicasService;

        public BL_HistoriasClinicas(
            IDAL_HistoriasClinicas dal,
            IDAL_Administrativo dalAdminService,
            IDAL_CitasMedicas dalCitasMedicasService)
        {
            this.dal = dal;
            this.dalAdminService = dalAdminService;
            this.dalCitasMedicasService = dalCitasMedicasService;
        }

        public List<ConsultaMedica> getConsultasMedicas()
        {
            var consultasMedicas = dal.getConsultasMedicas();

            if (consultasMedicas.Count == 0)
            {
                return null;
            }

            // Decripta las URLs en la capa de negocio
            foreach (var consulta in consultasMedicas)
            {
                foreach (var estudio in consulta.Estudios)
                {
                    estudio.ImagenUrl = AES.TryDecrypt(estudio.ImagenUrl);
                }
            }

            return consultasMedicas;
        }

        public ConsultaMedica getConsultaMedica(long id)
        {
            Console.WriteLine($"Obteniendo consulta médica con ID: {id}");
            return dal.getConsultaMedica(id);
        }

        public ConsultaMedicaCompletaDTO getConsultaMedicaCompleta(long id, long idCita)
        {
            var consultaMedica = dal.getConsultaMedica(id);
            if (consultaMedica == null) return null;

            var citaMedica = dalCitasMedicasService.getCitaMedicaById(idCita);
            if (citaMedica == null) return null;

            string pacienteDesId = AES.Decrypt(citaMedica.PacienteId);
            long pacienteId = long.Parse(pacienteDesId);
            var paciente = dalAdminService.GetPacienteById(pacienteId);
            if (paciente == null) return null;

            return new ConsultaMedicaCompletaDTO
            {
                ConsultaMedica = consultaMedica,
                CitaMedica = citaMedica,
                Paciente = paciente
            };
        }

        public ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica)
        {
            if (string.IsNullOrEmpty(consultaMedica.Descripcion))
            {
                throw new ArgumentException("La descripción no puede estar vacía");
            }

            if (string.IsNullOrEmpty(consultaMedica.Diagnostico))
            {
                throw new ArgumentException("El diagnóstico no puede estar vacío");
            }

            if (consultaMedica.CitaMedicaId == 0)
            {
                throw new ArgumentException("La consulta médica debe estar asociada a una cita médica");
            }

            var nuevaConsultaMedica = new ConsultaMedicaDTO
            {
                Descripcion = consultaMedica.Descripcion,
                Diagnostico = consultaMedica.Diagnostico,
                CitaMedicaId = consultaMedica.CitaMedicaId,
            };

            return dal.createConsultaMedica(nuevaConsultaMedica);
        }

        public ConsultaMedica createConsultaMedicaSD(long consultaMedicaId)
        {
            if (consultaMedicaId == 0)
            {
                throw new ArgumentException("La consulta médica debe estar asociada a una cita médica");
            }
            return dal.createConsultaMedicaSD(consultaMedicaId);
        }

        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica)
        {
            if (consultaMedica == null)
            {
                throw new ArgumentException("La consulta médica no puede ser nula");
            }
            return dal.updateConsultaMedica(consultaMedica);
        }

        public ConsultaMedica deleteConsultaMedica(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID proporcionado no es válido");
            }

            return dal.deleteConsultaMedica(id);
        }

        public ConsultaMedica addReceta(int idConsultaMedica, Receta receta)
        {
            if (receta == null)
            {
                throw new ArgumentException("La receta no puede ser nula");
            }
            return dal.addReceta(idConsultaMedica, receta);
        }

        public ConsultaMedica updateReceta(int idConsultaMedica, Receta receta)
        {
            if (receta == null)
            {
                throw new ArgumentException("La receta no puede ser nula");
            }
            return dal.updateReceta(idConsultaMedica, receta);
        }

        public ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta)
        {
            return dal.deleteReceta(idConsultaMedica, idReceta);
        }

        public async Task<ConsultaMedica> addEstudio(int idConsultaMedica, long idCita, Estudio estudio)
        {
            if (estudio == null)
            {
                throw new ArgumentException("El estudio no puede ser nulo");
            }

            DateOnly fechaRealizado = estudio.FechaRealizado ?? new DateOnly();
            Random random = new Random();
            int diasAdicionales = random.Next(1, 30);
            DateOnly fechaResultado = fechaRealizado.AddDays(diasAdicionales);

            var datosEstudio = new EstudioDTO
            {
                Nombre = estudio.Nombre,
                Descripcion = estudio.Descripcion,
                FechaRealizado = fechaRealizado,
                FechaResultado = fechaResultado
            };

            var consultaMedica = getConsultaMedica(idConsultaMedica);
            var citaMedica = dalCitasMedicasService.getCitaMedicaById(idCita);
            string pacienteDesId = AES.Decrypt(citaMedica.PacienteId ?? "");
            long pacienteId = long.Parse(pacienteDesId);
            var paciente = dalAdminService.GetPacienteById(pacienteId);

            // Generación de PDF y subida a S3
            PdfGenerator pdfGenerator = new PdfGenerator();
            byte[] pdf = pdfGenerator.GeneratePdf(datosEstudio, paciente);
            using var pdfStream = new MemoryStream(pdf);
            var s3Service = new S3Service();
            string pdfFileName = $"{idConsultaMedica}_{DateTime.UtcNow.Ticks}.pdf";
            string pdfUrl = await s3Service.UploadFileAsync(pdfStream, pdfFileName, "application/pdf");

            string encryptedPdf = AES.Encrypt(pdfUrl);

            estudio.ImagenUrl = encryptedPdf;
            estudio.FechaResultado = fechaResultado;

            // Agrega el estudio a la consulta médica y retorna el objeto completo
            return dal.addEstudio(idConsultaMedica, estudio);
        }


        public ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio)
        {
            if (estudio == null)
            {
                throw new ArgumentException("El estudio no puede ser nulo");
            }
            return dal.updateEstudio(idConsultaMedica, estudio);
        }

        public ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio)
        {
            return dal.deleteEstudio(idConsultaMedica, idEstudio);
        }

        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl)
        {
            return dal.addResultadoEstudio(idConsultaMedica, idEstudio, fechaResultado, imagenUrl);
        }

        public object GetHistoriaClinica(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            var paciente = dalAdminService.GetPacienteByDNI(dni);
            if (paciente == null)
            {
                return null;
            }

            var citasMedicas = dalCitasMedicasService.GetCitasMedicasByPacienteId(paciente.Id, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);

            List<ConsultaMedicaConCitaDTO> consultasMedicasConCitas = new List<ConsultaMedicaConCitaDTO>();
            foreach (var cita in citasMedicas)
            {
                var consulta = getConsultaMedica(cita.ConsultaMedicaId ?? 0);
                consultasMedicasConCitas.Add(new ConsultaMedicaConCitaDTO
                {
                    ConsultaMedica = consulta,
                    CitaMedica = cita
                });
            }
            // Para obtener el total de citas, útil para calcular el número total de páginas
            int totalCitas = dalCitasMedicasService.CountCitasMedicasByPacienteId(paciente.Id, fechaInicio, fechaFin, orden, especialidadesIds);

            return new
            {
                ConsultasMedicasConCitas = consultasMedicasConCitas,
                Paciente = paciente,
                TotalItems = totalCitas,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCitas / pageSize)
            };
        }

        public ConsultaMedica GuardarConsulta(long id, long idCita)
        {
            var consultaMedica = getConsultaMedica(id);
            if (consultaMedica == null)
            {
                return null;
            }

            var cita = dalCitasMedicasService.getCitaMedicaById(idCita);
            if (cita == null)
            {
                return null;
            }

            var citaDTO = new CitaMedicaDTO
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                Estado = "Completada",
                ConsultaMedicaId = consultaMedica.Id,
                PacienteId = cita.PacienteId
            };

            dalCitasMedicasService.updateCitaMedica(citaDTO);

            return consultaMedica;
        }

        public List<Medicamento> getMedicamentos()
        {
            return dal.getMedicamentos();
        }
    }
}
