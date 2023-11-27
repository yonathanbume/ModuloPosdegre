using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TermInform
{
    public class TeacherTermInfoTemplate
    {
        //
        public string TeacherId { get; set; }
        public string Teacher { get; set; }
        //
        public string Course { get; set; }
        public string Section { get; set; }
        public Guid? SectionId { get; set; }
    }
}
