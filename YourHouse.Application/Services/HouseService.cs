using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _repository;

        public HouseService(IRepository<House> repository)
        {
            _repository = repository;
        }

        public async Task AddHouseAsync(HouseDto HouseDto)
        {
            try
            {
                var house = new House()
                {

                };

                await _repository.AddAsync(house);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteHouseAsync(int id)
        {
            try
            {
                var house = await _repository.GetByIdAsync(id);

                if (house != null)
                {
                    _repository.DeleteAsync(house);
                    await _repository.SaveChangeAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HouseDto>> GetAllHouseAsync()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HouseDto?> GetHouseByIdAsync(int id)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateHouse(HouseDto houseDto)
        {
            try
            {
                var house = await _repository.GetByIdAsync(houseDto.ArticleId);

                if (house != null)
                {
                    house.BedRoom = houseDto.BedRoom;
                    house.BathRoom = houseDto.BathRoom;
                    house.Floors = houseDto.Floors;
                }

                _repository.UpdateAsync(house);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
