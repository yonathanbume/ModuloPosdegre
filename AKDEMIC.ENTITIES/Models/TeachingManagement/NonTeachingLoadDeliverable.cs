using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class NonTeachingLoadDeliverable : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid NonTeachingLoadId { get; set; }
        public NonTeachingLoad NonTeachingLoad { get; set; }

        public byte Status { get; set; } = ConstantHelpers.NON_TEACHING_LOAD_DELIVERABLE.STATUS.PENDING;
        public string Name { get; set; }
        public string FileUrl { get; set; }
    }
}
