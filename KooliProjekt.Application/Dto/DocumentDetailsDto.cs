using System;

namespace KooliProjekt.Application.Dto
{
    public class DocumentDetailsDto
    {
        public int DocumentId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public AppointmentDto Appointment { get; set; }
        public DoctorDto Doctor { get; set; }
    }
}
