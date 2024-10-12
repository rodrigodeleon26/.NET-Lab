using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class ConsultasMedicas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Descripcion { get; set; } = "-- Sin Descripción --";

        [Required]
        public string Diagnostico { get; set; } = "-- Sin Diagnóstico --";
    }
}
