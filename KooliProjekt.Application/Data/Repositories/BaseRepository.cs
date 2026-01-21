using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    // 28.11
    // Repositoride baasklass (pakub CRUD toiminguid)
    public abstract class BaseRepository<T> where T : Entity
    {
        protected ApplicationDbContext DbContext { get; private set; }

        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // CRUD

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync(T entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                // Check if entity exists in database by its primary key
                var keyValues = entry.Metadata.FindPrimaryKey()
                    .Properties
                    .Select(p => entry.Property(p.Name).CurrentValue)
                    .ToArray();

                var existingEntity = await DbContext.Set<T>().FindAsync(keyValues);

                if (existingEntity != null)
                {
                    // Entity exists, detach the existing and update with new values
                    DbContext.Entry(existingEntity).State = EntityState.Detached;
                    DbContext.Set<T>().Update(entity);
                }
                else
                {
                    await DbContext.Set<T>().AddAsync(entity);
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                DbContext.Set<T>().Update(entity);
            }
            else
            {
                await DbContext.Set<T>().AddAsync(entity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
