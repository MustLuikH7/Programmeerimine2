using System;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public bool IsPaid { get; set; }
    }
}
