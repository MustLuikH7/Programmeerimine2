using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class DoctorSchedule : Entity
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }


        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}
