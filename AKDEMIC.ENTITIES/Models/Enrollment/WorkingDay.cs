using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class WorkingDay
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid TermId { get; set; }
        public DateTime RegisterDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? Endtime { get; set; }
        public byte Status { get; set; } = ConstantHelpers.WORKING_DAY.STATUS.NORMAL;
        public ApplicationUser User { get; set; }
        public Term Term { get; set; }

        [NotMapped]
        public bool IsTemp { get; set; }
        [NotMapped]
        public DateTime RegisterDateLocal => RegisterDate.ToDefaultTimeZone();
    }
}
