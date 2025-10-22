using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}
