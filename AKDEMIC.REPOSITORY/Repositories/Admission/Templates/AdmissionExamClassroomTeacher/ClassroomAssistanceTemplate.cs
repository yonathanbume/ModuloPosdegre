using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomTeacher
{
    public class ClassroomAssistanceTemplate
    {
        public Guid? id { get;  set; }
        public Guid classroomId { get;  set; }
        public string classroom { get;  set; }
        public string building { get;  set; }
        public string campus { get;  set; }
        public string teacherId { get;  set; }
        public string teacherName { get;  set; }
        public bool @checked { get;  set; }
        public bool isPrincipal { get;  set; }
        public bool isToday { get;  set; }
        public bool ShowAssistance { get;  set; }
        public string PictureUrl { get; set; }
    }
}
