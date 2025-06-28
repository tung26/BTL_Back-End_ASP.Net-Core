using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<AccountDto?> GetAccountByEmailAsync(string email);
        Task<IEnumerable<AccountDto>> GetAllAccountAsync();
        Task AddAccountAsync(AccountDto accountDto);
        Task UpdateAccount(AccountDto accountDto);
        Task DeleteAccountAsync(int id);
        Task<bool> IsValidAccount(string email, string password);
    }
}
