using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public sealed class SectionTemplateA
    {
        public string AcademicDepartment { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Cycle { get; set; }
        public byte AcademicYear { get; set; }
        public double TheoricalHours { get; set; }
        public double VirtualHours { get; set; } 
        public double SeminarHours { get; set; } 
        public double PracticalHours { get; set; }
        public double TotalHours { get; set; }
        public string Group { get; set; }
        public int NumberStudents { get; set; }
        public bool IsElective { get; set; }
        /// <summary>
        /// nombre de docente de planta
        /// Si el profesor a cargo del curso pertenece a la escuela su nombre va en docente de planta
        /// </summary>
        public string PlantTeacher { get; set; }
        public string TeacherFromAnotherSchool { get; set; }
        public string SchoolOfOrigin { get; set; }

        public List<string> Teacher { get; set; }
        public List<string> AcademicDepartmentTeacher { get; set; }

        public Guid sectionId { get; set; }
        public Guid careerId { get; set; }
        public bool IsDirectedCourse { get; set; }
        public string Career { get; set; }
    }
    public class SectionSubstituteExamViewModel
    {
        public Guid id { get; set; }
        public Guid courseTermId { get; set; }
        public string code { get; set; }
        public string career { get; set; }
        public string name { get; set; }
        public string academicYear { get; set; }
        public string modality { get; set; }
        public string teachers { get; set; }
        public string groupcycle { get; set; }
        public string group { get; set; }
    }
}