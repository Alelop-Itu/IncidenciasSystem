using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting; // Necesario para mapear rutas
using IncidenciasSystem.Models;
using Newtonsoft.Json;

namespace IncidenciasSystem.Services
{
    public static class DataService
    {
        // El archivo se guardará en la carpeta App_Data del proyecto
        private static readonly string _path = HostingEnvironment.MapPath("~/App_Data/incidencias.json");

        public static List<Incidencia> Cargar()
        {
            if (!File.Exists(_path)) return new List<Incidencia>();
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Incidencia>>(json) ?? new List<Incidencia>();
        }

        public static void Guardar(List<Incidencia> datos)
        {
            var json = JsonConvert.SerializeObject(datos, Formatting.Indented);
            File.WriteAllText(_path, json);
        }

        public static int GenerarId(List<Incidencia> datos)
        {
            return datos.Any() ? datos.Max(x => x.Id) + 1 : 1;
        }
    }
}