using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseSyllabus : Entity, IKeyNumber
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid TermId { get; set; }    
        public string Summary { get; set; }
        public string GraduateProfile { get; set; }
        public string Competences { get; set; }
        public string LearningAchievement { get; set; }
        public string TeachingMethodologicalStrategies { get; set; }
        public string MethodologicalLearningStrategies { get; set; }
        public string MethodologicalResearchStrategies { get; set; }
        public string MethodologicalStrategiesOfSocialResponsability { get; set; }
        public string VirtualTeachingMethodologicalStrategies { get; set; }
        public string LearningProductPresentationDate { get; set; }
        public string LearningProductEntity { get; set; }
        public string DidacticMaterials { get; set; }
        public string BasicBibliographicReferences { get; set; }
        public string ComplementaryBibliographicReferences { get; set; }
        public string ElectronicBibliographicReferences { get; set; }
        public string IntellectualProductionBibliographicReferences { get; set; }
        public string Raiting { get; set; }

        //Tempdata
        public string Features { get; set; }
        public string LearningEnvironment { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCondition { get; set; }
        public string TeacherSpeciality { get; set; }

        public Course Course { get; set; }
        public Term Term { get; set; }
        public ICollection<CourseUnit> ListCourseUnit { get; set; }
        public ICollection<CourseSyllabusWeek> CourseSyllabusWeeks { get; set; }
        public ICollection<CourseSyllabusTeacher> CourseSyllabusTeachers { get; set; }
        public ICollection<CourseSyllabusObservation> CourseSyllabusObservations { get; set; }
    }
}
