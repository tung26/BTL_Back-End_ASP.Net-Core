using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IChungCuService
    {
        Task<ChungCuDto?> GetChungCuByIdAsync(int id);
        Task<IEnumerable<ChungCuDto>> GetAllChungCuAsync();
        Task AddChungCuAsync(ChungCuDto ChungCuDto);
        Task UpdateChungCu(ChungCuDto chungCuDto);
        Task DeleteChungCuAsync(int id);
    }
}
