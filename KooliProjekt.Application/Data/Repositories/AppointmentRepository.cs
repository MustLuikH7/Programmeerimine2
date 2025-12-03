using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Appointment> GetByIdAsync(int id)
        {
            return await DbContext
                .Appointments
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .Include(a => a.Invoice)
                .Include(a => a.Documents)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
