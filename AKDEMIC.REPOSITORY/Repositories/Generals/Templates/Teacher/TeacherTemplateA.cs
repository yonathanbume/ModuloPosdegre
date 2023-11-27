using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher
{
    public sealed class TeacherTemplateA
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Dedication { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public int LaborCondition { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? FacultyId { get; set; }
    }
}