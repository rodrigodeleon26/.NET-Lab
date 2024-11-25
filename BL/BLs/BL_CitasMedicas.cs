using BL.IBLs;
using DAL.IDALs;
using DAL.Models;
using Shared;
using System.Collections.Generic;
using System.Text.Json;

namespace BL.BLs
{
    public class BL_CitasMedicas : IBL_CitasMedicas
    {
        private readonly IDAL_CitasMedicas dal;
        private readonly IDAL_Administrativo dalAdmin;

        public BL_CitasMedicas(
            IDAL_CitasMedicas dal,
            IDAL_Administrativo dalAdmin)
        {
            this.dal = dal;
            this.dalAdmin = dalAdmin;
        }

        // Obtener todas las citas médicas
        public List<CitaMedicaDTO> getCitasMedicas()
        {
            List<CitaMedica> listaCitas = dal.getCitasMedicas();
            List<CitaMedicaDTO> listaCitasDTO = new List<CitaMedicaDTO>();
            foreach (CitaMedica cita in listaCitas)
            {
                string pacienteDesId = AES.Decrypt(cita.PacienteId);
                long pacienteId = long.Parse(pacienteDesId);
                Paciente paciente = dalAdmin.GetPacienteById(pacienteId);
                CitaMedicaDTO citaMedicaDTO = new CitaMedicaDTO
                {
                    Id = cita.Id,
                    Fecha = cita.Fecha,
                    Estado = cita.Estado,
                    Calendario = cita.Calendario,
                    CalendarioId = cita.CalendarioId,
                    ConsultaMedicaId = cita.ConsultaMedicaId,
                    PacienteId = cita.PacienteId,
                    Paciente = paciente
                };
                listaCitasDTO.Add(citaMedicaDTO);
            }
            return listaCitasDTO;
        }

        // Citas medicas por especialidad
        public List<CitaMedicaDTO> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha)
        {
            List<CitaMedica> listaCitas = dal.getCitasMedicasPorEspecialidad(nombreEspecialidad, numPagina, fecha);
            List<CitaMedicaDTO> listaCitasDTO = new List<CitaMedicaDTO>();
            foreach (CitaMedica cita in listaCitas)
            {
                string pacienteDesId = AES.Decrypt(cita.PacienteId);
                long pacienteId = long.Parse(pacienteDesId);
                Paciente paciente = dalAdmin.GetPacienteById(pacienteId);
                CitaMedicaDTO citaMedicaDTO = new CitaMedicaDTO
                {
                    Id = cita.Id,
                    Fecha = cita.Fecha,
                    Estado = cita.Estado,
                    Calendario = cita.Calendario,
                    CalendarioId = cita.CalendarioId,
                    ConsultaMedicaId = cita.ConsultaMedicaId,
                    PacienteId = cita.PacienteId,
                    Paciente = paciente
                };
                listaCitasDTO.Add(citaMedicaDTO);
            }
            return listaCitasDTO;
        }

        public bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha)
        {
            return dal.HayMasCitasMedicas(nombreEspecialidad, numPagina, fecha);
        }

        // Obtener una cita médica por ID
        public CitaMedicaDTO getCitaMedicaById(long id)
        {
            CitaMedica cita = dal.getCitaMedicaById(id);
            string pacienteDesId = AES.Decrypt(cita.PacienteId);
            long pacienteId = long.Parse(pacienteDesId);
            Paciente paciente = dalAdmin.GetPacienteById(pacienteId);
            CitaMedicaDTO citaMedicaDTO = new CitaMedicaDTO
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                Estado = cita.Estado,
                Calendario = cita.Calendario,
                CalendarioId = cita.CalendarioId,
                ConsultaMedicaId = cita.ConsultaMedicaId,
                PacienteId = cita.PacienteId,
                Paciente = paciente
            };
            return citaMedicaDTO;
        }

        // Crear una nueva cita médica
        public CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId)
        { 
            return dal.createCitaMedica(nuevaCita, calendarioId, pacienteId);
        }

        // Actualizar una cita médica existente
        public void updateCitaMedica(CitaMedicaDTO citaActualizada)
        {
            dal.updateCitaMedica(citaActualizada);

            //obtener la cita medica actualizada
            CitaMedica cita = dal.getCitaMedicaById(citaActualizada.Id);

            //en caso de que el estado sea completa o noAsistida se crea la factura para la cita
            if (cita.Estado == "Completada" || cita.Estado == "NoAsistida")
            {
                Console.WriteLine("voy a crear la factura");
                //obtener el paciente
                string pacienteDesId = AES.Decrypt(cita.PacienteId);
                long pacienteId = long.Parse(pacienteDesId);
                Paciente paciente = dalAdmin.GetPacienteById(pacienteId);

                Copago copago = dalAdmin.GetCopagoById(cita.CopagoId);
                //obtener el precio cuya FechaInicio corresponda con el dia actual

                DateTime hoy = DateTime.Today;
                Precio precio = copago.Precios
                    .Where(p => p.FechaInicio <= hoy)
                    .OrderByDescending(p => p.FechaInicio)
                    .FirstOrDefault();

                Articulo articulo = copago.Articulo;

                float precioBase = precio.PrecioBase;
                Console.WriteLine("precio base: " + precioBase);

                Factura factura = new Factura
                {
                    Monto = precioBase, //tengo que hacer lo del copago aun lpm
                    Pago = false,
                    Fecha = DateTime.Today,
                    FechaPago = null,
                    Descripcion = $"Pago por consulta medica de tipo: {articulo.Nombre}, a la fecha {cita.Fecha.ToString("dd/MM/yyyy HH:mm:ss")}",
                    Paciente = paciente,
                    PagoPayPal = null,
                };

                Console.WriteLine("voy a crear la factura");
                //muestro la factura en json
                var facturaJson = JsonSerializer.Serialize(factura);
                Console.WriteLine(facturaJson);

                //crear la factura
                dalAdmin.AddFactura(factura);
            }
        }

        // Eliminar una cita médica por ID
        public void deleteCitaMedica(int id)
        {
            dal.deleteCitaMedica(id);
        }

        public List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            return dal.GetCitasMedicasByPacienteId(pacienteId, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);
        }

        public int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            return dal.CountCitasMedicasByPacienteId(pacienteId, fechaInicio, fechaFin, orden, especialidadesIds);
        }

        public Paciente getPacienteByCedula(string cedula)
        {
            return dalAdmin.GetPacienteByDNI(cedula);
        }

        public Calendario getCalendarioById(long id)
        {
            return dalAdmin.GetCalendarioById(id);
        }

        public long getCopagoBySeguroEspecialidadArticulo(Copago copago)
        {
            return dalAdmin.getIdByFilds(copago);
        }
    }
}


