using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DeanFacultiesViewModels
{
    public class DeanFacultyViewModel
    {
        public Guid Id { get; set; }
        public string DeanGrade { get; set; }
        public string DeanId{ get; set; }
        public string SecretaryId { get; set; }
        public string Resolution { get; set; }
        public string AdministrativeAssistantId { get; set; }
        public IFormFile ResolutionFile { get; set; }
    }
}
