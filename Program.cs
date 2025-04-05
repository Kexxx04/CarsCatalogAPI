using CarsCatalog2.Data;
using Microsoft.EntityFrameworkCore;
using CarsCatalog2.Infraestructure.Repositories;
using CarsCatalog2.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Agregar el repositorio a la inyección de dependencias
builder.Services.AddScoped<ICarRepository, CarRepository>();

// Agregar conexión a la base de datos
builder.Services.AddDbContext<CarsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Agregar middleware de manejo de errores
app.UseMiddleware<CarsCatalog2.Middlewares.ErrorHandlingMiddleware>();

// Configurar el pipeline de HTTP (middlewares, etc.)
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddLogging(config =>
{
    config.AddConsole(); // Si usas consola
    config.AddDebug(); // Si usas Debug
});

// Agregar el middleware de logs
app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();