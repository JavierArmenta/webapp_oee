using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("Botones")]
    public class Boton
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del botón es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre del Botón")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código del botón es requerido")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El departamento es requerido")]
        [Display(Name = "Departamento")]
        public int DepartamentoOperadorId { get; set; }

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última Activación")]
        public DateTime? FechaUltimaActivacion { get; set; }

        // Relación con DepartamentoOperador
        [ForeignKey("DepartamentoOperadorId")]
        public virtual DepartamentoOperador DepartamentoOperador { get; set; } = null!;
    }
}
