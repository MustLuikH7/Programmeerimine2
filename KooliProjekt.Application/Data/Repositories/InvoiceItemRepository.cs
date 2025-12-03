using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class InvoiceItemRepository : BaseRepository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<InvoiceItem> GetByIdAsync(int id)
        {
            return await DbContext
                .InvoiceItems
                .Include(ii => ii.Invoice)
                .Where(ii => ii.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
