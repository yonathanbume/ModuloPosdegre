using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class WelfareAlert : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Status { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.WELFARE_ALERT.STATUS;
        public string Subject { get; set; }
        public string Commentary { get; set; }
    }
}
