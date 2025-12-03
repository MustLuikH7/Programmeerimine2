using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task SaveAsync(T list)
        {
            if (list.Id != 0)
            {
                DbContext.Set<T>().Update(list);
            }
            else
            {
                await DbContext.Set<T>().AddAsync(list);
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
