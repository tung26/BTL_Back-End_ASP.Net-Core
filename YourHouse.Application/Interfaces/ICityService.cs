using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface ICityService
    {
        Task<CityDto?> GetCityByIdAsync(int id);
        Task<IEnumerable<CityDto>> GetAllCityAsync();
        Task AddCityAsync(CityDto cityDto);
        void UpdateCity(City city);
        void DeleteCityAsync(int id);
    }
}
