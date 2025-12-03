using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Document> GetByIdAsync(int id)
        {
            return await DbContext
                .Documents
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
