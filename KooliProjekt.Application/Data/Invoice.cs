using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }


        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }


        [Required]
        public DateTime IssuedAt { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsPaid { get; set; }


        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }
    }
}
