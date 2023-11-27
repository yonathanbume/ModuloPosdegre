using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class Sede
    {
        public Guid Id { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public District District { get; set; }
        public Guid DistrictId { get; set; }
    }
}
