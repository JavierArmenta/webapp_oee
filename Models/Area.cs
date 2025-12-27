using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("Areas")]
    public class Area
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del área es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre del Área")]
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

        // Relación uno a muchos con Líneas
        public ICollection<Linea> Lineas { get; set; } = new List<Linea>();
    }
}
