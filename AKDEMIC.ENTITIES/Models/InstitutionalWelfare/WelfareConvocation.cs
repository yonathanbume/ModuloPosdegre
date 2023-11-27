using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class WelfareConvocation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PostulationStartDate { get; set; }
        public DateTime PostulationEndDate { get; set; }

        public ICollection<WelfareConvocationRequirement> Requirements { get; set; }
        public ICollection<WelfareConvocationFormat> Formats { get; set; }
        public ICollection<WelfareConvocationPostulant> Postulants { get; set; }
    }
}
