using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<UserDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<UserDetailsDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserDetailsDto>();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.UserId <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Users
                .Include(u => u.Invoices)
                .Where(u => u.UserId == request.UserId)
                .Select(u => new UserDetailsDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Invoices = u.Invoices.Select(i => new InvoiceDto
                    {
                        InvoiceId = i.InvoiceId,
                        AppointmentId = i.AppointmentId,
                        DoctorId = i.DoctorId,
                        UserId = i.UserId,
                        IssuedAt = i.IssuedAt,
                        IsPaid = i.IsPaid
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}