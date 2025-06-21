using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Services
{
    public class CityService : ICityService
    {
        private readonly IRepository<City> _repository;
        public CityService(IRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task AddCityAsync(CityDto cityDto)
        {
            var city = new City()
            {
                CityId = cityDto.CityId,
                CityName = cityDto.CityName,
            };

            await _repository.AddAsync(city);
            await _repository.SaveChangeAsync();
        }

        public async void DeleteCityAsync(int id)
        {
            var city = await _repository.GetByIdAsync(id);
            if(city != null)
            {
                _repository.DeleteAsync(city);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<CityDto>> GetAllCityAsync()
        {
            var cities = await _repository.GetAllAsync();
            return cities.Select(c => new CityDto()
            {
                CityId = c.CityId,
                CityName = c.CityName,
            });
        }

        public async Task<CityDto?> GetCityByIdAsync(int id)
        {
            var city = await _repository.GetByIdAsync(id);
            //if (city != null)
            //{
                return city == null ? null : new CityDto()
                {
                    CityId = city.CityId,
                    CityName = city.CityName,
                };
            //}
        }

        public async void UpdateCity(City city)
        {
            _repository.UpdateAsync(city);
            await _repository.SaveChangeAsync();
        }
    }
}
