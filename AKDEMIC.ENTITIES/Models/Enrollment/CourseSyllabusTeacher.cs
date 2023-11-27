using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseSyllabusTeacher
    {
        public Guid Id { get; set; }
        public Guid CourseSyllabusId { get; set; }
        public string TeacherId { get; set; }
        public string TempTeacherName { get; set; }
        public string Condition { get; set; }
        public string Speciality { get; set; }
        public CourseSyllabus CourseSyllabus { get; set; }
        public Teacher Teacher { get; set; }
    }
}
