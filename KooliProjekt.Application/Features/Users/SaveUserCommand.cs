using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommand : IRequest<OperationResult>
    {
        public int UserId { get; set; }

     
        public string FirstName { get; set; }

      
        public string LastName { get; set; }

      
        public string Email { get; set; }

        
        public string PasswordHash { get; set; }

        public string Phone { get; set; }
    }
}