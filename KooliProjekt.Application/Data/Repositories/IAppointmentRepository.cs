using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(int id);
        Task SaveAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
    }
}
