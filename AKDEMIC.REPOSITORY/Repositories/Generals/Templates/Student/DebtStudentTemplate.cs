using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class DebtStudentTemplate
    {
        public string Code { get;  set; }
        public string Document { get;  set; }
        public string Name { get;  set; }
        public string AdmissionType { get;  set; }
        public string Career { get;  set; }
        public string Faculty { get;  set; }
        public string Date { get; set; }
        public decimal Debt { get; set; }
        public string  UserId { get; set; }
    }
}
