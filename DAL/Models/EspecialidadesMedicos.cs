using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class EspecialidadesMedicos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public long EspecialidadId { get; set; }

        [ForeignKey(nameof(EspecialidadId))]
        public virtual Especialidades Especialidad { get; set; }

        [Required]
        public long MedicoId { get; set; }
        
        [ForeignKey(nameof(MedicoId))]
        public virtual Medicos Medico { get; set; }
    }
}
