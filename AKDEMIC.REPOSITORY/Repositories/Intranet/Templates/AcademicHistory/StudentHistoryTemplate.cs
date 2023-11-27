using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class StudentHistoryTemplate
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Term { get; set; }
        public decimal Grade { get; set; }
        public string Observations { get; set; }
        public string Career { get; set; }
        public bool Approved { get; set; }
        public byte Type { get; set; }
    }
}
