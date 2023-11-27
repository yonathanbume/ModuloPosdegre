using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class Sector
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public ICollection<Company> Companies { get; set; }
    }
}