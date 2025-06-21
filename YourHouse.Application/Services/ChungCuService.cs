using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Services
{
    public class ChungCuService : IChungCuService
    {
        private readonly IRepository<ChungCu> _repository;

        public ChungCuService(IRepository<ChungCu> repository)
        {
            _repository = repository;
        }

        //public Task AddChungCuAsync(ChungCuDto ChungCuDto)
        //{
        //    var chungCu = new ChungCu()
        //    {

        //    };
        //}

        public async Task DeleteChungCuAsync(int id)
        {
            var chungCu = await _repository.GetByIdAsync(id);

            if (chungCu != null)
            {
                _repository.DeleteAsync(chungCu);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<ChungCuDto>> GetAllChungCuAsync()
        {
            var chungCus = await _repository.GetAllAsync();

            return chungCus.Select(x => new ChungCuDto()
            {
                ArticleId = x.ArticleId,
                Floor = x.Floor,
                BathRoom = x.BathRoom,
                BedRoom = x.BedRoom,
                MaxPerson = x.MaxPerson,
                WaterPrice = x.WaterPrice,
                ElectricPrice = x.ElectricPrice,
            });
        }

        public async Task<ChungCuDto?> GetChungCuByIdAsync(int id)
        {
            var chungCu = await _repository.GetByIdAsync(id);

            return chungCu == null ? null : new ChungCuDto()
            {
                ArticleId = chungCu.ArticleId,
                Floor = chungCu.Floor,
                MaxPerson = chungCu.MaxPerson,
                WaterPrice = chungCu.WaterPrice,
                ElectricPrice = chungCu.ElectricPrice,
                BathRoom = chungCu.BathRoom,
                BedRoom = chungCu.BedRoom
            };
        }

        public async Task UpdateChungCu(ChungCu chungCu)
        {
            _repository.UpdateAsync(chungCu);
            await _repository.SaveChangeAsync();
        }
    }
}
