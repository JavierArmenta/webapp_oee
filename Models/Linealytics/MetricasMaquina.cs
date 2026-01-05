using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("MetricasMaquina", Schema = "linealytics")]
    public class MetricasMaquina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MaquinaId { get; set; }

        [Required]
        public int TurnoId { get; set; }

        public int? ProductoId { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        public int TiempoDisponibleMinutos { get; set; }

        public int TiempoProduccionMinutos { get; set; }

        public int TiempoParoMinutos { get; set; }

        public int UnidadesProducidas { get; set; }

        public int UnidadesDefectuosas { get; set; }

        public int UnidadesBuenas { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DisponibilidadPorcentaje { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal RendimientoPorcentaje { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal CalidadPorcentaje { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal OeePorcentaje { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        public bool Cerrada { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("MaquinaId")]
        public virtual Maquina Maquina { get; set; } = null!;

        [ForeignKey("TurnoId")]
        public virtual Turno Turno { get; set; } = null!;

        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }

        public virtual ICollection<RegistroParo> RegistrosParos { get; set; } = new List<RegistroParo>();
    }
}
