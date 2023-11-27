using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.TeacherViewModels
{
    public class TeacherViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string PicturePath { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }
        public int Sex { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public IFormFile Picture { get; set; }
        public Guid? AcademicDepartmentId { get; set; }
        public string Roles { get; set; }
        public string UserWeb { get; set; }
    }
}
