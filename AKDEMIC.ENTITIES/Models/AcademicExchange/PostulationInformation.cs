using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class PostulationInformation
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public Guid? CareerApplyId { get; set; }
        public Career CareerApply { get; set; }
    }
}
