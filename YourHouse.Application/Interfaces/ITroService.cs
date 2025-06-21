using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface ITroService
    {
        Task<TroDto?> GetTroByIdAsync(int id);
        Task<IEnumerable<TroDto>> GetAllTroAsync();
        //Task AddTroAsync(TroDto TroDto);
        Task UpdateTro(Tro tro);
        Task DeleteTroAsync(int id);
    }
}
