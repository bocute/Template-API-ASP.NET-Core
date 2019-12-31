using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Interfaces
{
    public interface IRefreshTokenRepository<T> : IDisposable where T : class
    {
        T GetByRefreshToken(string id, string token);
        void Add(T obj);
        void Remove(string id);
    }
}
