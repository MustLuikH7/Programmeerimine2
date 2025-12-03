using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Data.Repositories
{
    // 28.11
    // ToDo listide repository klass
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        // Lisa siia spetsiifilisemad meetodid,
        // mis on seotud Kasutajatega

        // BaseRepository ei tea, et Get peab tooma kaasa ka Arved
        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbContext
                .Users
                .Include(user => user.Invoices)
                .Where(user => user.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
