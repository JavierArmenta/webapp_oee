using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("Dispositivos", Schema = "linealytics")]
    public class Dispositivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MaquinaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? CodigoDispositivo { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [MaxLength(100)]
        public string? TipoDispositivo { get; set; } // PLC, Sensor, Gateway, etc.

        public int? UltimoContador { get; set; }

        public DateTime? FechaUltimaLectura { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("MaquinaId")]
        public virtual Maquina Maquina { get; set; } = null!;
    }
}
