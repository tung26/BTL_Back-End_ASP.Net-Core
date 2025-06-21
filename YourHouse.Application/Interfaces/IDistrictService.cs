using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IDistrictService
    {
        Task<DistrictDto?> GetDistrictByIdAsync(int id);
        Task<IEnumerable<DistrictDto>> GetAllDistrictAsync();
        Task AddDistrictAsync(DistrictDto cistrictDto);
        void UpdateDistrict(District cistrict);
        void DeleteDistrictAsync(int id);
    }
}
