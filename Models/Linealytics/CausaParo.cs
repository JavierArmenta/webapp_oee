using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Linealytics
{
    [Table("CausasParo", Schema = "linealytics")]
    public class CausaParo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoriaParoId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [MaxLength(50)]
        public string? CodigoInterno { get; set; }

        public bool RequiereMantenimiento { get; set; } = false;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("CategoriaParoId")]
        public virtual CategoriaParo CategoriaParo { get; set; } = null!;

        public virtual ICollection<RegistroParo> RegistrosParos { get; set; } = new List<RegistroParo>();
    }
}
