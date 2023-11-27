using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CurriculumArea
{
    public class CurriculumAreaTemplate
    {
        public string Name { get;  set; }
        public List<AdmissionExamTemplate> AdmissionExam { get;  set; }
    }
    public class AdmissionExamTemplate
    {
        public Guid Id { get;  set; }
        public Guid CurriculumId { get;  set; }
        public string Name { get;  set; }
    }
}
