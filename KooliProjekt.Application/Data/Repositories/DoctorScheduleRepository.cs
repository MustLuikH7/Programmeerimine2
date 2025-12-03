using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class DoctorScheduleRepository : BaseRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        public DoctorScheduleRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<DoctorSchedule> GetByIdAsync(int id)
        {
            return await DbContext
                .DoctorSchedules
                .Include(ds => ds.Doctor)
                .Where(ds => ds.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
