using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommand : IRequest<OperationResult>
    {
        public int DoctorId { get; set; }

      
        public string FirstName { get; set; }

    
        public string LastName { get; set; }

       
        public string Email { get; set; }

       
        public string PasswordHash { get; set; }

        public string Specialty { get; set; }
    }
}