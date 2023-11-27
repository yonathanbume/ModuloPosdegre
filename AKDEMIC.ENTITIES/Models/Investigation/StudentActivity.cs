using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class StudentActivity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public byte Status { get; set; }

        /// <summary>
        /// 0 por revisar
        /// 1 con observaciones
        /// 2 calificado
        /// </summary>
        public decimal? Qualification { get; set; }
        public List<StudentFeedbackActivity> StudentFeedbackActivities { get; set; }
        public List<StudentActivityFile> StudentActivityFiles { get; set; }
    }
}
