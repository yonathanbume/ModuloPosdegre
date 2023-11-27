using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularCourseGroupAssistance : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }
        public ExtracurricularCourseGroup Group { get; set; }

        public DateTime RegisterDate { get; set; }
        
        [NotMapped]
        public string RegisterDateText => RegisterDate.ToLocalDateFormat();

        public ICollection<ExtracurricularCourseGroupAssistanceStudent> GroupAssistanceStudents { get; set; }
    }
}
