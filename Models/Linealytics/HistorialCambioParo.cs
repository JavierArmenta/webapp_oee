using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("HistorialCambiosParos", Schema = "linealytics")]
    public class HistorialCambioParo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RegistroParoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CampoModificado { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ValorAnterior { get; set; }

        [MaxLength(500)]
        public string? ValorNuevo { get; set; }

        [Required]
        [MaxLength(100)]
        public string UsuarioModifica { get; set; } = string.Empty;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Motivo { get; set; }

        // Relaciones
        [ForeignKey("RegistroParoId")]
        public virtual RegistroParo RegistroParo { get; set; } = null!;
    }
}
