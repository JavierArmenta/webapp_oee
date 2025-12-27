using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("Estaciones")]
    public class Estacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la estación es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre de la Estación")]
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

        // Relación con Línea (muchos a uno)
        [Required(ErrorMessage = "La línea es requerida")]
        [Display(Name = "Línea")]
        public int LineaId { get; set; }

        [ForeignKey("LineaId")]
        public Linea Linea { get; set; } = null!;

        // Relación uno a muchos con Máquinas
        public ICollection<Maquina> Maquinas { get; set; } = new List<Maquina>();
    }
}
