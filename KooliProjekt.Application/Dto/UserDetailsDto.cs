using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
        public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
    }
}
