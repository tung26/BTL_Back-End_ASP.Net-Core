using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Services
{
    public class AccountService : IAccountService
    {

        private readonly IRepository<Account> _repository;

        public AccountService(IRepository<Account> repository)
        {
            _repository = repository;
        }

        public async Task AddAccountAsync(AccountDto accountDto)
        {
            var account = new Account()
            {
                FullName = accountDto.FullName,
                PasswordHash = accountDto.PasswordHash,
                Email = accountDto.Email,
                Phone = accountDto.Phone,
                RoleId = accountDto.RoleId
            };

            await _repository.AddAsync(account);
            await _repository.SaveChangeAsync();
        }

        public async void DeleteAccountAsync(int id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account != null)
            {
                _repository.DeleteAsync(account);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _repository.GetByIdAsync(id);
            //if (account != null)
            //{
                return account == null ? null : new AccountDto()
                {
                    AccountId = account.AccountId,
                    FullName = account.FullName,
                    PasswordHash = account.PasswordHash,
                    Email = account.Email,
                    Phone = account.Phone,
                    RoleId = account.RoleId
                };
            //}
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts.Select(account => new AccountDto()
            {
                AccountId = account.AccountId,
                FullName = account.FullName,
                PasswordHash = account.PasswordHash,
                Email = account.Email,
                Phone = account.Phone,
                RoleId = account.RoleId
            });
        }

        public async void UpdateAccount(AccountDto accountDto)
        {
            var account = await _repository.GetByIdAsync(accountDto.AccountId);

            account.FullName = accountDto.FullName;
            account.PasswordHash = accountDto.PasswordHash;
            account.Email = accountDto.Email;
            account.Phone = accountDto.Phone;
            account.RoleId = accountDto.RoleId;

            Console.WriteLine(account.FullName);

            _repository.UpdateAsync(account);
            await _repository.SaveChangeAsync();
        }

        public async Task<AccountDto?> GetAccountByEmailAsync(string email)
        {
            var accounts = await _repository.GetAllAsync();
            var acc = accounts.Where(b => b.Email == email).FirstOrDefault();
            return acc == null ? null : new AccountDto()
            {
                AccountId = acc.AccountId,
                FullName = acc.FullName,
                PasswordHash = acc.PasswordHash,
                Email = acc.Email,
                Phone = acc.Phone,
                RoleId = acc.RoleId
            };
        }

        public async Task<bool> IsValidAccount(string email, string password)
        {
            var account = await _repository.GetAllAsync();
            bool isSuccess = account.Any(b => b.Email == email && b.PasswordHash == password);
            return isSuccess;
        }
    }
}
