using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class TroService : ITroService
    {
        private readonly IRepository<Tro> _repository;

        public TroService(IRepository<Tro> repository)
        {
            _repository = repository;
        }

        public async Task AddTroAsync(TroDto TroDto)
        {
            try
            {
                var tro = new Tro()
                {

                };

                await _repository.AddAsync(tro);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteTroAsync(int id)
        {
            try
            {
                var tro = await _repository.GetByIdAsync(id);

                if (tro != null)
                {
                    _repository.DeleteAsync(tro);
                    await _repository.SaveChangeAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TroDto>> GetAllTroAsync()
        {
            try
            {
                var tros = await _repository.GetAllAsync();

                return tros.Select(x => new TroDto()
                {
                    ArticleId = x.ArticleId,
                    Floor = x.Floor,
                    MaxPerson = x.MaxPerson,
                    WaterPrice = x.WaterPrice,
                    ElectricPrice = x.ElectricPrice,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TroDto?> GetTroByIdAsync(int id)
        {
            try
            {
                var tro = await _repository.GetByIdAsync(id);

                return tro == null ? null : new TroDto()
                {
                    ArticleId = tro.ArticleId,
                    Floor = tro.Floor,
                    MaxPerson = tro.MaxPerson,
                    WaterPrice = tro.WaterPrice,
                    ElectricPrice = tro.ElectricPrice,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateTro(TroDto troDto)
        {
            try
            {
                var tro = await _repository.GetByIdAsync(troDto.ArticleId);

                if (tro != null)
                {
                    tro.Floor = troDto.Floor;
                    tro.MaxPerson = troDto.MaxPerson;
                    tro.WaterPrice = troDto.WaterPrice;
                    tro.ElectricPrice = troDto.ElectricPrice;
                }

                _repository.UpdateAsync(tro);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
