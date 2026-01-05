using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("LecturasContador", Schema = "linealytics")]
    public class LecturaContador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MaquinaId { get; set; }

        [Required]
        public int DispositivoId { get; set; }

        public int? ProductoId { get; set; }

        public int? MetricasMaquinaId { get; set; }

        [Required]
        public int Contador { get; set; }

        public int? ContadorAnterior { get; set; }

        public int? UnidadesProducidas { get; set; }

        public DateTime FechaLectura { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Relaciones
        [ForeignKey("MaquinaId")]
        public virtual Maquina Maquina { get; set; } = null!;

        [ForeignKey("DispositivoId")]
        public virtual Dispositivo Dispositivo { get; set; } = null!;

        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("MetricasMaquinaId")]
        public virtual MetricasMaquina? MetricasMaquina { get; set; }
    }
}
