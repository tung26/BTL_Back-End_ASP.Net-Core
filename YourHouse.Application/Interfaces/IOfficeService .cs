using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IOfficeService
    {
        Task<OfficeDto?> GetOfficeByIdAsync(int id);
        Task<IEnumerable<OfficeDto>> GetAllOfficeAsync();
        //Task AddOfficeAsync(OfficeDto OfficeDto);
        Task UpdateOffice(Office office);
        Task DeleteOfficeAsync(int id);
    }
}
