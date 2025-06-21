using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

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
            var district = new District()
            {
                DistrictName = cistrictDto.DistrictName,
                CityId = cistrictDto.CityId,
            };

            await _repository.AddAsync(district);
            await _repository.SaveChangeAsync();
        }

        public async void DeleteDistrictAsync(int id)
        {
            var district = await _repository.GetByIdAsync(id);
            if( district != null )
            {
                _repository.DeleteAsync(district);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<DistrictDto>> GetAllDistrictAsync()
        {
            var districtes = await _repository.GetAllAsync();
            return districtes.Select(x => new DistrictDto()
            {
                DistrictName = x.DistrictName,
                CityId = x.CityId,
                DistrictId = x.DistrictId,
            });
        }

        public async Task<DistrictDto?> GetDistrictByIdAsync(int id)
        {
            var district = await _repository.GetByIdAsync(id);
            return district == null ? null : new DistrictDto()
            {
                DistrictId = district.DistrictId,
                CityId = district.CityId,
                DistrictName = district.DistrictName,
            };
        }

        public async void UpdateDistrict(District cistrict)
        {
            _repository.UpdateAsync(cistrict);
            await _repository.SaveChangeAsync();
        }
    }
}
