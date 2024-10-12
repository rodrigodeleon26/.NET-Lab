using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Consultorios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public int Piso { get; set; }

        public virtual ICollection<CitasMedicas> CitasMedicas { get; set; } = new List<CitasMedicas>(); // Usar virtual si necesitas Lazy Loading
    }
}
