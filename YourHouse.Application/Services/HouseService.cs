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
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _repository;

        public HouseService(IRepository<House> repository)
        {
            _repository = repository;
        }

        //public Task AddHouseAsync(HouseDto HouseDto)
        //{
        //    var house = new House()
        //    {

        //    };
        //}

        public async Task DeleteHouseAsync(int id)
        {
            var house = await _repository.GetByIdAsync(id);

            if (house != null)
            {
                _repository.DeleteAsync(house);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<HouseDto>> GetAllHouseAsync()
        {
            var houses = await _repository.GetAllAsync();

            return houses.Select(x => new HouseDto()
            {
                ArticleId = x.ArticleId,
                BedRoom = x.BedRoom,
                BathRoom = x.BedRoom,
                Floors = x.Floors,
            });
        }

        public async Task<HouseDto?> GetHouseByIdAsync(int id)
        {
            var house = await _repository.GetByIdAsync(id);

            return house == null ? null : new HouseDto()
            {
                ArticleId = id,
                BedRoom = house.BedRoom,
                BathRoom = house.BathRoom,
                Floors = house.Floors
            };
        }

        public async Task UpdateHouse(House house)
        {
            _repository.UpdateAsync(house);
            await _repository.SaveChangeAsync();
        }
    }
}
