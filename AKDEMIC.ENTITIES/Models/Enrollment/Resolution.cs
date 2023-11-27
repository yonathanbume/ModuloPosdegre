using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Resolution : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? KeyValue { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime IssueDate { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public ICollection<Activity> Activity { get; set; }
        public ICollection<EnrollmentShift> EnrollmentShifts { get; set; }
        public ICollection<NonActivity> NonActivity { get; set; }
    }
}
