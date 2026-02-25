using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDetailsDto
    {
        public int InvoiceId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public bool IsPaid { get; set; }

        public AppointmentDto Appointment { get; set; }
        public DoctorDto Doctor { get; set; }
        public UserDto User { get; set; }
        public List<InvoiceItemDto> InvoiceItems { get; set; } = new List<InvoiceItemDto>();
    }
}
