using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("Lineas")]
    public class Linea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la línea es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre de la Línea")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [StringLength(50)]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con Área (muchos a uno)
        [Required(ErrorMessage = "El área es requerida")]
        [Display(Name = "Área")]
        public int AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual Area? Area { get; set; }  // ← CAMBIO: Hacerlo nullable

        // Relación uno a muchos con Estaciones
        public virtual ICollection<Estacion> Estaciones { get; set; } = new List<Estacion>();
    }
}