using BL.IBLs;
using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLs
{
    public class BL_Administrativo_Service : IBL_Administrativo
    {
        private readonly IDAL_Administrativo dal;

        public BL_Administrativo_Service(IDAL_Administrativo dal)
        {
            this.dal = dal;
        }

        public void activarContrato(long id)
        {
            throw new NotImplementedException();
        }

        public void addArticulo(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public void addCalendario(Calendario calendario)
        {
            throw new NotImplementedException();
        }

        public void addCitaMedica(CitaMedica citaMedica)
        {
            throw new NotImplementedException();
        }

        public void addConsultorio(Consultorio consultorio)
        {
            throw new NotImplementedException();
        }

        public void addContrato(Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public void addCopago(Copago copago)
        {
            throw new NotImplementedException();
        }

        public void addEspecialidad(Especialidad especialidad)
        {
            throw new NotImplementedException();
        }

        public void addFactura(Factura factura)
        {
            throw new NotImplementedException();
        }

        public void addMedico(Medico medico)
        {
            throw new NotImplementedException();
        }

        public void addPaciente(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public void addPrecio(Precio precio)
        {
            throw new NotImplementedException();
        }

        public void addSeguroMedico(SeguroMedico seguroMedico)
        {
            throw new NotImplementedException();
        }

        public void asignarEspecialidad(long medId, long espId)
        {
            throw new NotImplementedException();
        }

        public void ContratarSeguroMedico(long idPaciente, long idSeguroMedico)
        {
            throw new NotImplementedException();
        }

        public void crearCalendario(long medId, long espId, long conId, TimeSpan horaInicio, TimeSpan horaFin, int tiempo, int cant, string[] dias)
        {
            throw new NotImplementedException();
        }

        public void deleteArticulo(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteCalendario(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteCitaMedica(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteConsultorio(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteContrato(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteCopago(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteEspecialidad(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteFactura(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteMedico(long id)
        {
            throw new NotImplementedException();
        }

        public void deletePaciente(long id)
        {
            throw new NotImplementedException();
        }

        public void deletePrecio(long id)
        {
            throw new NotImplementedException();
        }

        public void deleteSeguroMedico(long id)
        {
            throw new NotImplementedException();
        }

        public Articulo getArticuloById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Articulo> getArticulos()
        {
            throw new NotImplementedException();
        }

        public List<Articulo> getArticulosFiltrados(string filtro)
        {
            throw new NotImplementedException();
        }

        public Calendario getCalendarioById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Calendario> getCalendarios()
        {
            throw new NotImplementedException();
        }

        public CitaMedica getCitaMedicaById(long id)
        {
            throw new NotImplementedException();
        }

        public List<CitaMedica> getCitasMedicas()
        {
            throw new NotImplementedException();
        }

        public Consultorio getConsultorioById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Consultorio> getConsultorios()
        {
            throw new NotImplementedException();
        }

        public Contrato getContratoById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Contrato> getContratos()
        {
            throw new NotImplementedException();
        }

        public Copago getCopagoById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Copago> getCopagos()
        {
            throw new NotImplementedException();
        }

        public Especialidad getEspecialidadById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Especialidad> getEspecialidades()
        {
            throw new NotImplementedException();
        }

        public Factura getFacturaById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Factura> getFacturas()
        {
            throw new NotImplementedException();
        }

        public Medico getMedicoById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Medico> getMedicos()
        {
            throw new NotImplementedException();
        }

        public List<Medico> getMedicosPaginadosYFiltrados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public List<Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public Paciente getPacienteByDNI(string dni)
        {
            return dal.GetPacienteByDNI(dni);
        }

        public Paciente getPacienteById(long id)
        {
            return dal.GetPacienteById(id);
        }

        public List<Paciente> getPacientes()
        {
            throw new NotImplementedException();
        }

        public Precio getPrecioById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Precio> getPrecios()
        {
            throw new NotImplementedException();
        }

        public SeguroMedico getSeguroMedicoById(long id)
        {
            throw new NotImplementedException();
        }

        public List<SeguroMedico> getSegurosMedicos()
        {
            throw new NotImplementedException();
        }

        public void updateArticulo(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public void updateCalendario(Calendario calendario)
        {
            throw new NotImplementedException();
        }

        public void updateCitaMedica(CitaMedica citaMedica)
        {
            throw new NotImplementedException();
        }

        public void updateConsultorio(Consultorio consultorio)
        {
            throw new NotImplementedException();
        }

        public void updateContrato(Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public void updateCopago(Copago copago)
        {
            throw new NotImplementedException();
        }

        public void updateEspecialidad(Especialidad especialidad)
        {
            throw new NotImplementedException();
        }

        public void updateFactura(Factura factura)
        {
            throw new NotImplementedException();
        }

        public void updateMedico(Medico medico)
        {
            throw new NotImplementedException();
        }

        public void updatePaciente(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public void updatePrecio(Precio precio)
        {
            throw new NotImplementedException();
        }

        public void updateSeguroMedico(SeguroMedico seguroMedico)
        {
            throw new NotImplementedException();
        }

        public bool cedulaDuplicada(string cedula)
        {
            throw new NotImplementedException();
        }

        public bool emailDuplicado(string email)
        {
            throw new NotImplementedException();
        }
    }
}
