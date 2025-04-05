using System.Collections.Generic;
using System.Threading.Tasks;
using CarsCatalog2.Models;

namespace CarsCatalog2.Infraestructure.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetCarsAsync(int page, int pageSize);
        Task<Car?> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task<IEnumerable<Car>> FilterCarsAsync(string? model, decimal? priceMin, decimal? priceMax, decimal? millMin, decimal? millMax, int page, int pageSize);

    }
}


