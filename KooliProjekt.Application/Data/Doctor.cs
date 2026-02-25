using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Specialty { get; set; }

        public virtual ICollection<DoctorSchedule> Schedules { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public Doctor()
        {
            Schedules = new HashSet<DoctorSchedule>();
            Appointments = new HashSet<Appointment>();
            Invoices = new HashSet<Invoice>();
            Documents = new HashSet<Document>();
        }
    }
}