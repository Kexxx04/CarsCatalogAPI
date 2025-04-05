using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CarsCatalog2.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;// Guarda la referencia al siguiente middleware
        private readonly ILogger<LoggingMiddleware> _logger;// Permite registrar logs


        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;// Guardamos el middleware siguiente
            _logger = logger;// Guardamos el logger
        }

        public async Task Invoke(HttpContext context)
        {
            //Solo registramos logs para el endpoint GET /api/Cars
            if (context.Request.Path.StartsWithSegments("/api/Cars") && context.Request.Method == "GET")
            {
                var stopwatch = Stopwatch.StartNew();//Inicia cronometro
                await _next(context); // Llamamos al siguiente middleware y lo ejecutamos hasta que termine
                stopwatch.Stop(); //Detiene el cronómetro cuando la petición finaliza.

                _logger.LogInformation($"[LOG] {context.Request.Path} - {stopwatch.ElapsedMilliseconds}ms");//Guardamos el mensaje en los logs
            }
            else
            {
                await _next(context);
            }
        }
    }
}
