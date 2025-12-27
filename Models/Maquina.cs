using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("Maquinas")]
    public class Maquina
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la máquina es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre de la Máquina")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [StringLength(50)]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }

        [StringLength(100)]
        [Display(Name = "Número de Serie")]
        public string? NumeroSerie { get; set; }

        [StringLength(100)]
        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }

        [StringLength(100)]
        [Display(Name = "Fabricante")]
        public string? Fabricante { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con Estación (muchos a uno)
        [Required(ErrorMessage = "La estación es requerida")]
        [Display(Name = "Estación")]
        public int EstacionId { get; set; }

        [ForeignKey("EstacionId")]
        public Estacion Estacion { get; set; } = null!;
    }
}
