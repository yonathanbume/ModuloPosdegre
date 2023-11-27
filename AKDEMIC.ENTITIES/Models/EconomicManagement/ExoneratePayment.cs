using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ExoneratePayment : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Observations { get; set; }

        public string File { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
