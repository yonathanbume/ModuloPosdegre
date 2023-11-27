using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.AcademicRecordGenerator.Models
{
    public class AcademicRecordModel
    {
        public string UserProcedureCode { get; set; }
        public string LogoImg { get; set; }
        public string UserLoggedIn { get; set; }
        public string QrCode { get; set; }
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        public StudentModel Student { get; set; }
        public List<TermModel> Terms { get; set; }
    }
}
