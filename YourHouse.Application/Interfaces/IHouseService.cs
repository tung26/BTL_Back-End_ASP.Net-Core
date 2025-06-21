using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IHouseService
    {
        Task<HouseDto?> GetHouseByIdAsync(int id);
        Task<IEnumerable<HouseDto>> GetAllHouseAsync();
        //Task AddHouseAsync(HouseDto HouseDto);
        Task UpdateHouse(House house);
        Task DeleteHouseAsync(int id);
    }
}
