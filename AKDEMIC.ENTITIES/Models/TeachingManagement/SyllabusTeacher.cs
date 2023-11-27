using System;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class SyllabusTeacher : ITimestamp
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string TeacherId { get; set; }
        public Guid SyllabusRequestId { get; set; }
        public Guid CourseTermId { get; set; }
        public byte Status { get; set; } = ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS;
        public DateTime? PresentationDate { get; set; }
        public SyllabusRequest SyllabusRequest{ get; set; }
        public CourseTerm CourseTerm { get; set; }

        [ForeignKey("TeacherId")]
        public ApplicationUser Teacher { get; set; }

        [NotMapped]
        public bool IsDigital => string.IsNullOrEmpty(Url);
    }
}
