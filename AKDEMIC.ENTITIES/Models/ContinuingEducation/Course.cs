using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class Course : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
        public string Place { get; set; }
        public int OrganizerEntity { get; set; } //AKDEMIC.CORE.Helpers.ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY
        public Guid EntityId { get; set; } //en base a la constante se guardará el Guid de la entidad entre, Career,Faculty,AcademicDepartment
        public Guid CourseTypeId { get; set; }
        public CourseType CourseType { get; set; }
        public Guid CourseAreaId { get; set; }
        public CourseArea CourseArea { get; set; }
        public string Duration { get; set; }
        public string Modality { get; set; }
        public string ImageUrl { get; set; }
        public string Investment { get; set; }
        public string PresentationUrl { get; set; }
        public ICollection<CourseExhibitor> Exhibitors { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
