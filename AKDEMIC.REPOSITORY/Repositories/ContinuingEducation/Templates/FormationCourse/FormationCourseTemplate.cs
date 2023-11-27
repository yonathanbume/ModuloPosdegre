using AKDEMIC.ENTITIES.Models.ContinuingEducation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationCourse
{
    public class FormationCourseTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
        public string Place { get; set; }
        public int OrganizerEntity { get; set; } //AKDEMIC.CORE.Helpers.ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY
        public string OrganizerEntityText { get; set; }
        public Guid EntityId { get; set; } //en base a la constante se guardará el Guid de la entidad entre, Career,Faculty,AcademicDepartment
        public string EntityName { get; set; }
        public Guid CourseTypeId { get; set; }
        public string CourseTypeName { get; set; }
        public Guid CourseAreaId { get; set; }
        public string CourseAreaName { get; set; }
        public string Duration { get; set; }
        public string Modality { get; set; }
        public string Investment { get; set; }
        public string ImageUrl { get; set; }
        public string PresentationUrl { get; set; }
    }
}
