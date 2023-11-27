using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TermInform
{
    public class TermInformTemplate
    {
        public Guid Id { get; set; }
        public string RequestType { get; set; }
        public string Type { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public List<TeacherTermInfoTemplate> Details { get; set; }
        public bool IsBySection { get; set; }
        public bool HasError { get; set; }
        public bool AnyByTerm { get; set; }
        public bool Completed { get; set; }
    }
}
