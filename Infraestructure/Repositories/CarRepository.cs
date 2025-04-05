using Microsoft.EntityFrameworkCore;
using CarsCatalog2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsCatalog2.Data;

namespace CarsCatalog2.Infraestructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarsDbContext _context;// Este es el contexto de la base de datos

        public CarRepository(CarsDbContext context)// Inicializamos el contexto para acceder a la base de datos
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetCarsAsync(int page, int pageSize)
        {
            return await _context.Cars //Esto accede a la tabla de autos en la base de datos a través de CarsDbContext.
              .Include(c => c.Brand) //Incluimos la marca de cada auto
              .Skip((page - 1) * pageSize) //Saltamos los autos ya mostrados
              .Take(pageSize) //Limitamos el numero de autos devueltos
              .ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _context.Cars
                     .Include(c => c.Brand) // Incluimos la marca del auto
                     .FirstOrDefaultAsync(c => c.Id == id); //Buscamos por Id
        }

        // Método en el repositorio para agregar un coche
        public async Task AddCarAsync(Car car)
        {
            // Verificar si el auto ya existe en la base de datos
            var carInDb = await _context.Cars.FindAsync(car.Id);
            if (carInDb != null)
            {
                // Si el auto ya existe, lanzamos un error
                throw new InvalidOperationException("Car with the same ID already exists in the database.");
            }

            // Verificar si el auto ya está siendo rastreado por el contexto
            var existingCar = _context.Cars.Local.FirstOrDefault(c => c.Id == car.Id);
            if (existingCar != null)
            {
                // Si el auto ya está siendo rastreado, desasociarlo para evitar el conflicto
                _context.Entry(existingCar).State = EntityState.Detached;
            }

            // Agregar el nuevo coche al contexto
            _context.Cars.Add(car);
            await _context.SaveChangesAsync(); // Guardamos los cambios en la base de datos
        }

        public async Task<IEnumerable<Car>> FilterCarsAsync(string? model, decimal? priceMin, decimal? priceMax, decimal? millMin, decimal? millMax, int page, int pageSize)
        {
            // Comenzamos la consulta base, incluyendo la relación con la tabla de marcas
            var query = _context.Cars
                .Include(c => c.Brand) // Incluye los datos de la marca del carro
                .AsQueryable(); // Convierte a IQueryable para permitir aplicar filtros dinámicos

            // Si se proporciona un modelo, se filtra por modelo (usando Contains para coincidencias parciales)
            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(c => c.Model.Contains(model));

            // Si se proporciona un precio mínimo, filtra los carros con precio mayor o igual
            if (priceMin.HasValue)
                query = query.Where(c => c.Price >= priceMin.Value);

            // Si se proporciona un precio máximo, filtra los carros con precio menor o igual
            if (priceMax.HasValue)
                query = query.Where(c => c.Price <= priceMax.Value);

            // Si se proporciona un kilometraje mínimo, filtra los carros con kilometraje mayor o igual
            if (millMin.HasValue)
                query = query.Where(c => c.Mileage >= millMin.Value);

            // Si se proporciona un kilometraje máximo, filtra los carros con kilometraje menor o igual
            if (millMax.HasValue)
                query = query.Where(c => c.Mileage <= millMax.Value);

            // Aplicamos paginación: saltamos los registros anteriores según la página y tomamos solo los del tamaño de página
            return await query
                .Skip((page - 1) * pageSize) // Saltamos los registros de páginas anteriores
                .Take(pageSize) // Tomamos solo los registros de la página actual
                .ToListAsync(); // Ejecutamos la consulta y obtenemos la lista
        }


    }

}