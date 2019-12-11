using RBTemplate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity<T>
    {
        void Add(T obj);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int? pageNumber, int pageSize);
        void Update(T obj);
        void Remove(Guid id);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
