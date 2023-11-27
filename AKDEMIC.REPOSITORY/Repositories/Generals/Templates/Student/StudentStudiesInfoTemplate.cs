using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentStudiesInfoTemplate
    {
        public Guid StudentId { get; set; }
        public string UserName { get; set; }
        public string DNI { get; set; }
        public string FullName { get; set; }
        public string CareerName { get; set; }
        public string WeightedAverageGrade { get; set; }
        public string UserPicture { get; set; }
        public string StudentState { get; set; }
        public string Email { get; set; }
        public string DegreeYear { get; set; }

        public StudyTemplate BachelorDegree { get; set; }
        public StudyTemplate ProfesionalTitleDegree { get; set; }
    }

    public class StudyTemplate
    {
        public string Year { get; set; }
        public string Resolution { get; set; }
        public string ResolutionDate { get; set; }
        public string Diploma { get; set; }
        public string DiplomaDate { get; set; }
    }
}
