using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.CompleteCurriculumGenerator.Models
{
    public class CompleteCurriculumModel
    {
        public string UserProcedureCode { get; set; }
        public string QrCode { get; set; }
        public string LogoImg { get; set; }
        public string UserLoggedIn { get; set; }
        public string HeaderTitle { get; set; }
        public StudentModel Student { get; set; }
        public List<CourseModel> Courses { get; set; }
    }
}
