using System;

namespace KooliProjekt.Application.Dto
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
