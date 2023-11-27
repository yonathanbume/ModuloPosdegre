using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationSection
{
    public class FormationSectionTemplate
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Code { get; set; }
        public int Vacancies { get; set; }
        public string InscriptionStart { get; set; }
        public string InscriptionEnd { get; set; }
        public string ClassStart { get; set; }
        public string ClassEnd { get; set; }
        public string CourseName { get; set; }
        public string CourseArea { get; set; }
        public string CourseType { get; set; }
        public string CourseImage { get; set; }
        public string CourseDescription { get; set; }
        public string CourseDuration { get; set; }
        public string CoursePlace { get; set; }
        public string CourseModality { get; set; }
        public string CourseInvestment { get; set; }
        public string CourseRequirements { get; set; }
    }
}
