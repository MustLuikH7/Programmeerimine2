using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Document : Entity
    {
        [Key]
        public int DocumentId { get; set; }



        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int DoctorId { get; set; }



        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(1024)]
        public string FilePath { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; }


        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}
