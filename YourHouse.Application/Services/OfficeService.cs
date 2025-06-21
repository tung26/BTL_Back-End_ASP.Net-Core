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
    public class OfficeService : IOfficeService
    {
        private readonly IRepository<Office> _repository;

        public OfficeService(IRepository<Office> repository)
        {
            _repository = repository;
        }

        //public Task AddOfficeAsync(OfficeDto OfficeDto)
        //{
        //    var office = new Office()
        //    {

        //    };
        //}

        public async Task DeleteOfficeAsync(int id)
        {
            var office = await _repository.GetByIdAsync(id);

            if (office != null)
            {
                _repository.DeleteAsync(office);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<OfficeDto>> GetAllOfficeAsync()
        {
            var offices = await _repository.GetAllAsync();

            return offices.Select(x => new OfficeDto()
            {
                ArticleId = x.ArticleId,
                Floor = x.Floor,
                DoorDrt = x.DoorDrt,
            });
        }

        public async Task<OfficeDto?> GetOfficeByIdAsync(int id)
        {
            var office = await _repository.GetByIdAsync(id);

            return office == null ? null : new OfficeDto()
            {
                ArticleId = id,
                Floor = office.Floor,
                DoorDrt = office.DoorDrt,
            };
        }

        public async Task UpdateOffice(Office office)
        {
            _repository.UpdateAsync(office);
            await _repository.SaveChangeAsync();
        }
    }
}
