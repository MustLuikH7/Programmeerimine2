using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Doctor> GetByIdAsync(int id)
        {
            return await DbContext
                .Doctors
                .Include(d => d.Schedules)
                .Include(d => d.Appointments)
                .Include(d => d.Invoices)
                .Include(d => d.Documents)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
