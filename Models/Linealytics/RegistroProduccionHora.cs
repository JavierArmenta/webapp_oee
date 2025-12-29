using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("RegistrosProduccionHora", Schema = "linealytics")]
    public class RegistroProduccionHora
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SesionProduccionId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        public int UnidadesProducidas { get; set; }

        public int UnidadesDefectuosas { get; set; }

        public int UnidadesBuenas { get; set; }

        public int TiempoProduccionMinutos { get; set; }

        public int TiempoParoMinutos { get; set; }

        public int? OperadorId { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("SesionProduccionId")]
        public virtual SesionProduccion SesionProduccion { get; set; } = null!;

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } = null!;

        [ForeignKey("OperadorId")]
        public virtual Operador? Operador { get; set; }
    }
}
