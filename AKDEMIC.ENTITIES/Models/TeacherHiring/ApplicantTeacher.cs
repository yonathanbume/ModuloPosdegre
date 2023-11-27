using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ApplicantTeacher : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ConvocationId { get; set; }
        public ApplicationUser User { get; set; }
        public Convocation Convocation { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
    }
}
