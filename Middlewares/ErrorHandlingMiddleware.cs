using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CarsCatalog2.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Ejecuta el siguiente middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ERROR] Ha ocurrido una excepción no controlada.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            int statusCode;
            string errorMessage;

            switch (exception)
            {
                case KeyNotFoundException: // Excepción cuando un recurso no se encuentra
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = "The requested resource was not found.";
                    break;
                case ArgumentException: // Excepción de argumentos inválidos
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = "Bad request. Please check the data sent.";
                    break;
                default: // Cualquier otro error no manejado
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "An internal server error occurred.";
                    break;
            }

            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new { status = statusCode, message = errorMessage });
            return response.WriteAsync(result);
        }
    }
}
