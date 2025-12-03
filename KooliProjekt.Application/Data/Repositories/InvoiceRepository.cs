using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Invoice> GetByIdAsync(int id)
        {
            return await DbContext
                .Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Doctor)
                .Include(i => i.User)
                .Include(i => i.InvoiceItems)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
