using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IInvoiceItemRepository
    {
        Task<InvoiceItem> GetByIdAsync(int id);
        Task SaveAsync(InvoiceItem invoiceItem);
        Task DeleteAsync(InvoiceItem invoiceItem);
    }
}
