using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class AppointmentDetailsDto
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public DoctorDto Doctor { get; set; }
        public UserDto User { get; set; }
        public InvoiceDto Invoice { get; set; }
        public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
