using System;

namespace KooliProjekt.Application.Dto
{
    public class DoctorScheduleDetailsDto
    {
        public int ScheduleId { get; set; }
        public int DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public DoctorDto Doctor { get; set; }
    }
}
