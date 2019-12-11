using RBTemplate.Domain.Interfaces;
using RBTemplate.Infra.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExampleContext _context;
        public UnitOfWork(ExampleContext context)
        {
            _context = context;
        }
        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
