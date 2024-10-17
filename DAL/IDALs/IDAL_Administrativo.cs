using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IDALs
{
    public interface IDAL_Administrativo
    {
        // Pacientes
        public List<Paciente> GetPacientes();
        public Paciente GetPacienteById(int id);
        public void AddPaciente(Paciente paciente);
        public void UpdatePaciente(Paciente paciente);
        public void DeletePaciente(int id);

        // Seguros Medicos
        public List<SeguroMedico> GetSegurosMedicos();
        public SeguroMedico GetSeguroMedicoById(int id);
        public void AddSeguroMedico(SeguroMedico seguroMedico);
        public void UpdateSeguroMedico(SeguroMedico seguroMedico);
        public void DeleteSeguroMedico(int id);

        // Contratos
        public List<Contrato> GetContratos();
        public Contrato GetContratoById(int id);
        public void AddContrato(Contrato contrato);
        public void UpdateContrato(Contrato contrato);
        public void DeleteContrato(int id);

        // Precios
        public List<Precio> GetPrecios();
        public Precio GetPrecioById(int id);
        public void AddPrecio(Precio precio);
        public void UpdatePrecio(Precio precio);
        public void DeletePrecio(int id);

        // Copagos
        public List<Copago> GetCopagos();
        public Copago GetCopagoById(int id);
        public void AddCopago(Copago copago);
        public void UpdateCopago(Copago copago);
        public void DeleteCopago(int id);


    }
}
