using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class WelfareConvocationPostulant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid WelfareConvocationId { get; set; }
        public WelfareConvocation WelfareConvocation { get; set; }
        public byte Status { get; set; }
        public DateTime? AdmissionDate { get; set; }
        
        public ICollection<WelfareConvocationPostulantFile> Files { get; set; }
    }
}
