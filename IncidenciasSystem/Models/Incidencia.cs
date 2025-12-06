using System;
using System.ComponentModel.DataAnnotations;

namespace IncidenciasSystem.Models
{
    public class Incidencia
    {
        // Campos Originales
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

        // === PROPIEDADES NUEVAS DE SOLICITANTE ===

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El área es obligatoria.")]
        public string Area { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; }

        // === PROPIEDADES NUEVAS DE ADMINISTRACIÓN Y ESTADO ===

        [Required]
        [RegularExpression("^(Abierto|En Proceso|Cancelado|Cerrado)$", ErrorMessage = "Estado debe ser Abierto, En Proceso, Cancelado o Cerrado.")]
        public string Estado { get; set; }

        public string ComentarioAdmin { get; set; }
    }
}