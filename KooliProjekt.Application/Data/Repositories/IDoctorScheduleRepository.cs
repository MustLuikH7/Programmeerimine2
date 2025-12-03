using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IDoctorScheduleRepository
    {
        Task<DoctorSchedule> GetByIdAsync(int id);
        Task SaveAsync(DoctorSchedule doctorSchedule);
        Task DeleteAsync(DoctorSchedule doctorSchedule);
    }
}
