using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<District> _repository;

        public DistrictService(IRepository<District> repository)
        {
            _repository = repository;
        }

        public async Task AddDistrictAsync(DistrictDto cistrictDto)
        {
            try
            {
                var district = new District()
                {
                    DistrictName = cistrictDto.DistrictName,
                    CityId = cistrictDto.CityId,
                };

                await _repository.AddAsync(district);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void DeleteDistrictAsync(int id)
        {
            try
            {
                var district = await _repository.GetByIdAsync(id);
                if (district != null)
                {
                    _repository.DeleteAsync(district);
                    await _repository.SaveChangeAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DistrictDto>> GetAllDistrictAsync()
        {
            try
            {
                var districtes = await _repository.GetAllAsync();
                return districtes.Select(x => new DistrictDto()
                {
                    DistrictName = x.DistrictName,
                    CityId = x.CityId,
                    DistrictId = x.DistrictId,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DistrictDto?> GetDistrictByIdAsync(int id)
        {
            try
            {
                var district = await _repository.GetByIdAsync(id);
                return district == null ? null : new DistrictDto()
                {
                    DistrictId = district.DistrictId,
                    CityId = district.CityId,
                    DistrictName = district.DistrictName,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void UpdateDistrict(District cistrict)
        {
            try
            {
                _repository.UpdateAsync(cistrict);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
