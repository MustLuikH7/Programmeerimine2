using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetByIdAsync(int id);
        Task SaveAsync(Doctor doctor);
        Task DeleteAsync(Doctor doctor);
    }
}
