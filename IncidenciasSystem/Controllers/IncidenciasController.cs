using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using IncidenciasSystem.Models;
using IncidenciasSystem.Services;

namespace IncidenciasSystem.Controllers
{
    public class IncidenciasController : ApiController
    {
        // Simulación de validación de usuario
        private bool EsAdmin()
        {
            // Busca el header "Rol-Usuario"
            if (Request.Headers.Contains("Rol-Usuario"))
            {
                var rol = Request.Headers.GetValues("Rol-Usuario").FirstOrDefault();
                return rol == "Admin";
            }
            return false;
        }

        // GET: api/Incidencias
        public IHttpActionResult Get()
        {
            var datos = DataService.Cargar();
            return Ok(datos);
        }

        // POST: api/Incidencias (Crear)
        public IHttpActionResult Post(Incidencia incidencia)
        {
            // Obtener el rol del encabezado
            string rol = Request.Headers.Contains("Rol-Usuario") ? Request.Headers.GetValues("Rol-Usuario").FirstOrDefault() : "";

            // PERMISO: Solo Admin y Usuario pueden crear (Se deniega si el rol es inválido o nulo)
            if (rol != "Admin" && rol != "Usuario")
            {
                // Devolvemos el mensaje que espera el frontend
                return Content(HttpStatusCode.Forbidden, "Permiso Denegado: Error de API.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var datos = DataService.Cargar();
            incidencia.Id = DataService.GenerarId(datos);
            incidencia.FechaCreacion = DateTime.Now;

            // Establecer estado y comentario por defecto para nuevas incidencias
            incidencia.Estado = "Abierto";
            incidencia.ComentarioAdmin = null;

            datos.Add(incidencia);
            DataService.Guardar(datos);

            return Ok(incidencia);
        }

        // PUT: api/Incidencias/5 (Editar y Cerrar)
        public IHttpActionResult Put(int id, Incidencia incidencia)
        {
            // Obtener el rol del encabezado
            string rol = Request.Headers.Contains("Rol-Usuario") ? Request.Headers.GetValues("Rol-Usuario").FirstOrDefault() : "";

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var datos = DataService.Cargar();
            var index = datos.FindIndex(x => x.Id == id);

            if (index == -1) return NotFound();

            var incidenciaExistente = datos[index];

            // --- LÓGICA DE PERMISOS DE EDICIÓN ---
            if (rol == "Admin")
            {
                // El administrador puede cambiar TODOS los campos, incluyendo Estado y Comentario.
                // Asumimos que todos los campos relevantes de la incidencia vienen en la solicitud.
                incidenciaExistente.Titulo = incidencia.Titulo;
                incidenciaExistente.Descripcion = incidencia.Descripcion;
                incidenciaExistente.Prioridad = incidencia.Prioridad;

                // El admin puede modificar el Estado y el Comentario
                incidenciaExistente.Estado = incidencia.Estado;
                incidenciaExistente.ComentarioAdmin = incidencia.ComentarioAdmin;
            }
            else if (rol == "Usuario")
            {
                // El usuario solo puede modificar su propia incidencia si está Abierta
                bool esSuIncidencia = incidenciaExistente.NombreUsuario == incidencia.NombreUsuario;
                bool estaAbierta = incidenciaExistente.Estado == "Abierto";

                if (!esSuIncidencia || !estaAbierta)
                {
                    return Content(HttpStatusCode.Forbidden, "Permiso Denegado: No puedes editar esta incidencia.");
                }

                // El usuario solo puede modificar campos básicos
                incidenciaExistente.Titulo = incidencia.Titulo;
                incidenciaExistente.Descripcion = incidencia.Descripcion;
                incidenciaExistente.Prioridad = incidencia.Prioridad;

                // No puede modificar Estado ni ComentarioAdmin
            }
            else
            {
                // Rol no reconocido o vacío
                return Content(HttpStatusCode.Forbidden, "Permiso Denegado: Rol inválido.");
            }
            // --- FIN DE LÓGICA DE PERMISOS ---

            DataService.Guardar(datos);
            return Ok(incidenciaExistente);
        }

        // DELETE: api/Incidencias/5
        public IHttpActionResult Delete(int id)
        {
            if (!EsAdmin()) return Content(HttpStatusCode.Forbidden, "Solo Administradores.");

            var datos = DataService.Cargar();
            var item = datos.FirstOrDefault(x => x.Id == id);

            if (item == null) return NotFound();

            datos.Remove(item);
            DataService.Guardar(datos);
            return Ok();
        }
    }
}