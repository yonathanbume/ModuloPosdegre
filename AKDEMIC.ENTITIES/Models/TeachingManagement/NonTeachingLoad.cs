using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class NonTeachingLoad
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Resolution { get; set; }
        public string Location { get; set; }
        public int Minutes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public Guid? RelatedCourseId { get; set; }
        public Guid? TermId { get; set; }
        public Course RelatedCourse { get; set; }
        public Term Term { get; set; }

        [NotMapped]
        public string ParsedStartDate => StartDate?.ToLocalDateFormat();

        [NotMapped]
        public string ParsedEndDate => EndDate?.ToLocalDateFormat();

        [NotMapped]
        public string Hours => TimeSpan.FromMinutes(Minutes).TotalHours.ToString("0.00");

        public Guid TeachingLoadTypeId { get; set; }
        public TeachingLoadType TeachingLoadType { get; set; }

        public Guid? TeachingLoadSubTypeId { get; set; }
        public TeachingLoadSubType TeachingLoadSubType { get; set; }

        public IEnumerable<NonTeachingLoadSchedule> NonTeachingLoadSchedules { get; set; }
        public IEnumerable<NonTeachingLoadActivity> NonTeachingLoadActivities { get; set; }
        public IEnumerable<NonTeachingLoadDeliverable> NonTeachingLoadDeliverables { get; set; }
    }
}
