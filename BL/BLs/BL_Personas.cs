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
    public class BL_Personas : IBL_Personas
    {
        private readonly IDAL_Personas dal;

        public BL_Personas(IDAL_Personas _dal)
        {
            dal = _dal;
        }

        public void AddPersona(Persona persona)
        {
            dal.AddPersona(persona);
        }

        public void DeletePersona(long id)
        {
            dal.DeletePersona(id);
        }

        public Persona GetPersona(long id)
        {
            return dal.GetPersona(id);
        }

        public List<Persona> GetPersonas()
        {
            return dal.GetPersonas();
        }

        public void UpdatePersona(Persona persona)
        {
            Persona result = dal.GetPersona(persona.Id);

            if (result == null)
            {
                throw new Exception("No existe una persona con id " + persona.Id);
            }

            if (string.IsNullOrEmpty(persona.Nombres))
            {
                throw new Exception("El nombre no puede estar vacío");
            }

            if (!persona.Documento.Equals(result.Documento))
            {
                if (dal.GetPersonas().Any(p => p.Documento.Equals(persona.Documento)))
                {
                    throw new Exception("Ya existe una persona con el documento " + persona.Documento);
                }
            }

            dal.UpdatePersona(persona);
        }
    }
}