using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class DoctorDetailsDto
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public List<DoctorScheduleDto> Schedules { get; set; } = new List<DoctorScheduleDto>();
        public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
        public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
        public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
