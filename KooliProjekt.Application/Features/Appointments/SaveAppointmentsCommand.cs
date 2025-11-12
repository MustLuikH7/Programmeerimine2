using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommand : IRequest<OperationResult>
    {
        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }


        public int UserId { get; set; }

        public DateTime AppointmentTime { get; set; }

    
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }
    }
}