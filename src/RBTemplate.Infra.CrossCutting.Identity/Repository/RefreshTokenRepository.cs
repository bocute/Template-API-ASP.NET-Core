using Microsoft.EntityFrameworkCore;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Infra.CrossCutting.Identity.Authorization;
using RBTemplate.Infra.CrossCutting.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Infra.CrossCutting.Identity.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository<RefreshTokenData>
    {
        protected RefreshTokenContext Db;
        protected DbSet<RefreshTokenData> DbSet;

        public RefreshTokenRepository(RefreshTokenContext context)
        {
            Db = context;
            DbSet = Db.Set<RefreshTokenData>();
        }

        public void Add(RefreshTokenData obj)
        {
            Remove(obj.UsuarioId);
            DbSet.Add(obj);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public RefreshTokenData GetByRefreshToken(string id, string token)
        {
            return DbSet.Where(r => r.UsuarioId == id && r.RefreshToken == token).FirstOrDefault();
        }

        public void Remove(string id)
        {
            var refreshToken = DbSet.Where(r => r.UsuarioId == id).FirstOrDefault();

            if (refreshToken != null)
            {
                DbSet.Remove(refreshToken);
                Db.SaveChanges();
            }
        }
    }
}
