using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(int id);
        Task SaveAsync(Document document);
        Task DeleteAsync(Document document);
    }
}
