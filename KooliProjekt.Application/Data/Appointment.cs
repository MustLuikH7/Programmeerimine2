using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Appointment : Entity
    {
        [Key]
        public int AppointmentId { get; set; }


        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }


        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }


        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual Invoice Invoice { get; set; }


        public virtual ICollection<Document> Documents { get; set; }

        public Appointment()
        {
            Documents = new HashSet<Document>();
        }
    }
}
