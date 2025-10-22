using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public bool IsPaid { get; set; }
    }
}
