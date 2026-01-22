using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class GetDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, OperationResult<DoctorScheduleDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorScheduleQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<DoctorScheduleDetailsDto>> Handle(GetDoctorScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DoctorScheduleDetailsDto>();

            if (request == null)
            {
                return result;
            }

            result.Value = await _dbContext
                .DoctorSchedules
                .Include(s => s.Doctor)
                .Where(s => s.ScheduleId == request.ScheduleId)
                .Select(s => new DoctorScheduleDetailsDto
                {
                    ScheduleId = s.ScheduleId,
                    DoctorId = s.DoctorId,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    Doctor = new DoctorDto
                    {
                        DoctorId = s.Doctor.DoctorId,
                        FirstName = s.Doctor.FirstName,
                        LastName = s.Doctor.LastName,
                        Email = s.Doctor.Email,
                        Specialty = s.Doctor.Specialty
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}