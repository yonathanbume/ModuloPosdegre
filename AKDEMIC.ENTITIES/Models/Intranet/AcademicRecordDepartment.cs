using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class AcademicRecordDepartment : Entity, ITimestamp
    {
        [Key]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Key]
        public Guid AcademicDepartmentId { get; set; }
        public AcademicDepartment AcademicDepartment { get; set; }
    }
}
