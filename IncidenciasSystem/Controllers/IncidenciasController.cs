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
            if (!EsAdmin()) return Content(HttpStatusCode.Forbidden, "Solo Administradores.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var datos = DataService.Cargar();
            incidencia.Id = DataService.GenerarId(datos);
            incidencia.FechaCreacion = DateTime.Now;

            datos.Add(incidencia);
            DataService.Guardar(datos);

            return Ok(incidencia);
        }

        // PUT: api/Incidencias/5 (Editar)
        public IHttpActionResult Put(int id, Incidencia incidencia)
        {
            if (!EsAdmin()) return Content(HttpStatusCode.Forbidden, "Solo Administradores.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var datos = DataService.Cargar();
            var index = datos.FindIndex(x => x.Id == id);

            if (index == -1) return NotFound();

            datos[index].Titulo = incidencia.Titulo;
            datos[index].Descripcion = incidencia.Descripcion;
            datos[index].Prioridad = incidencia.Prioridad;

            DataService.Guardar(datos);
            return Ok(datos[index]);
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