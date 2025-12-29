using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("Productos", Schema = "linealytics")]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public int TiempoCicloSegundos { get; set; }

        public int UnidadesPorCiclo { get; set; } = 1;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public virtual ICollection<SesionProduccion> SesionesProduccion { get; set; } = new List<SesionProduccion>();
        public virtual ICollection<RegistroProduccionHora> RegistrosProduccionHora { get; set; } = new List<RegistroProduccionHora>();
    }
}
