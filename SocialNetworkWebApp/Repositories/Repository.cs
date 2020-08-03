using Microsoft.EntityFrameworkCore;
using SocialNetworkWebApp.Data;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly SocialNetworkDBContext context;
        protected DbSet<T> entities;
        public Repository(SocialNetworkDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<int> InsertAsync(T entity)
        {
            await entities.AddAsync(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            entities.Update(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            T entity = entities.SingleOrDefault(s => s.Id == id);
            entities.Remove(entity);
            return await context.SaveChangesAsync();
        }
    }
}
