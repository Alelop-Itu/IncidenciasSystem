using System.Web.Http;
using System.Web.Http.Cors; // Namespace del paquete instalado

namespace IncidenciasSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 1. Habilitar CORS (Permite acceso desde cualquier origen por ahora)
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // 2. Rutas de API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 3. Forzar respuesta JSON en lugar de XML
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }
    }
}