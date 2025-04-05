using CarsCatalog2.Data;
using CarsCatalog2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using CarsCatalog2.Infraestructure.Repositories;
using System.Formats.Asn1;

namespace CarsCatalog2.Presentation.Controllers
{
    [Route("api/Cars")] //Anotacion que define la ruta base para todas las acciones dentro de este controlador, se pone sin el "Controller" del final
    [ApiController]//Se usa para indicar que esta clase es un controlador de API
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;// Hacemos uso del repositorio

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;// Inicializamos el contexto para acceder a la base de datos
        }

        [HttpGet]
        public async Task<ActionResult> GetCars([FromQuery] int page = 1, [FromQuery] int pageSize = 10) //Esto permite que el cliente pase parámetros a través de la URL. En este caso, los parámetros son page (número de página) y pageSize (número de autos por página). Si el cliente no los pasa, se usan los valores por defecto: page = 1 y pageSize = 10.
        {
            var cars = await _carRepository.GetCarsAsync(page, pageSize);  // Usamos el repositorio para obtener los autos
            return Ok(new { data = cars, status = 200, message = "Answer ok" });
        }

        // GET: api/cars/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCar(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);  // Usamos el repositorio para obtener el auto por ID

            if (car == null)//Si no encontramos el auto, devolvemos un error 404 (No encontrado) con un mensaje adecuado.
            {
                return NotFound(new { status = 404, message = "Car not found" });
            }

            return Ok(new { data = car, status = 200, message = "Answer ok" });//Si el auto es encontrado, devolvemos los detalles del auto con un mensaje de éxito.
        }

        // Método que maneja las solicitudes POST para crear un coche
        [HttpPost]
        public async Task<ActionResult> CreateCar(Car car)
        {
            // Validamos si los datos del auto son correctos
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = 400, message = "Invalid data" });
            }

            // Verificar si el auto ya existe por el Id en la base de datos
            var existingCar = await _carRepository.GetCarByIdAsync(car.Id);
            if (existingCar != null)
            {
                return Conflict(new { status = 409, message = "Car with this ID already exists." });
            }

            // Usamos el repositorio para agregar el auto
            await _carRepository.AddCarAsync(car);

            // Devolvemos una respuesta con el estado 201 (Creado)
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, new { data = car, status = 201, message = "Car created successfully" });
        }

        [HttpGet("filter")] // Define la ruta del endpoint como api/cars/filter
        public async Task<IActionResult> FilterCars(
             [FromQuery] string? model,           // Filtro opcional: modelo del carro
             [FromQuery] decimal? pricemin,       // Filtro opcional: precio mínimo
             [FromQuery] decimal? pricemax,       // Filtro opcional: precio máximo
             [FromQuery] decimal? millmin,        // Filtro opcional: kilometraje mínimo
             [FromQuery] decimal? millmax,        // Filtro opcional: kilometraje máximo
             [FromQuery] int page = 1,            // Número de página, por defecto 1
             [FromQuery] int pageSize = 10)       // Tamaño de página, por defecto 10
        {
             // Llama al repositorio para obtener los autos filtrados según los criterios
             var cars = await _carRepository.FilterCarsAsync(model, pricemin, pricemax, millmin, millmax, page, pageSize);

             // Devuelve la respuesta con el formato solicitado: datos, estado y mensaje
             return Ok(new
             {
                 data = cars,                          // Lista de autos obtenidos
                 status = 200,                         // Código HTTP 200 (OK)
                 message = "Filtered cars retrieved successfully" // Mensaje informativo
             });
        }

    }

}
