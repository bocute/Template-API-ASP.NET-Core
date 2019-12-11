using Microsoft.EntityFrameworkCore;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Domain.Models;
using RBTemplate.Infra.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Infra.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity<T>
    {
        protected ExampleContext Db;
        protected DbSet<T> DbSet;

        public Repository(ExampleContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }

        public async void Add(T obj)
        {
            await Task.FromResult(DbSet.Add(obj));
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? pageNumber, int pageSize)
        {
            return await PaginatedList<T>.CreateAsync(DbSet.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(DbSet.Find(id));
        }

        public async void Remove(Guid id)
        {
            await Task.FromResult(DbSet.Remove(DbSet.Find(id)));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async void Update(T obj)
        {
            await Task.FromResult(DbSet.Update(obj));
        }
    }
}
