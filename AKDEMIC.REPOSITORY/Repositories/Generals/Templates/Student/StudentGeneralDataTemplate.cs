using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentGeneralDataTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Email { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string BirthDate { get; set; }
        public string Dni { get; set; }
        public string DocumentType { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public IFormFile Picture { get; set; }
        public string PictureUrl { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public Guid FacultyId { get; set; }
        public Guid SelectedCareer { get; set; }
        public string AcademicProgramName { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public string UserWeb { get; set; }
        public byte RacialIdentity { get; set; }
        //public Guid? StudentScaleId { get; set; }
        public Guid? EnrollmentFeeId { get; set; }
    }
}
