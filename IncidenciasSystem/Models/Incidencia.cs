using System;
using System.ComponentModel.DataAnnotations;

namespace IncidenciasSystem.Models
{
    public class Incidencia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required]
        [RegularExpression("^(Baja|Media|Alta)$", ErrorMessage = "Prioridad debe ser Baja, Media o Alta.")]
        public string Prioridad { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}