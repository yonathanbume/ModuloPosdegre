using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class StudentUserProcedureDetail
    {
        public Guid Id { get; set; }
        public Guid? StudentSectionId { get; set; }
        public StudentSection StudentSection { get; set; }
        public Guid StudentUserProcedureId { get; set; }
        public StudentUserProcedure StudentUserProcedure { get; set; }
    }
}
