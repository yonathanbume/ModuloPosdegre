using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class FavoriteCompany : Entity, ITimestamp
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
