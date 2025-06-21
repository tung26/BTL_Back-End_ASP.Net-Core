using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure.Data;

namespace YourHouse.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        public readonly YourHousebContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(YourHousebContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void DeleteAsync(T entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToArrayAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();

        public void UpdateAsync(T entity) => _dbSet.Update(entity);
    }
}
