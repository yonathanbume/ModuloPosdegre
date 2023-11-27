using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ClinicHistory : Entity, ITimestamp
    {
        public Guid Id { get ; set; }
        public string UserId { get; set; } //Paciente
        public int TypeInsurance { get; set; }
        public string Workplace { get; set; }
        public string EmergencyFullName { get; set; }
        public string EmergencyPhone { get; set; }
        public ApplicationUser User { get; set; }

    }
}
