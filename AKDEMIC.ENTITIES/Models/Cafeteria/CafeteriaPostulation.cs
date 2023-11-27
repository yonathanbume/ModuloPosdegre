using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaPostulation
    {
        public Guid Id { get; set; }
        public Guid CafeteriaServiceTermId { get; set; }
        public CafeteriaServiceTerm CafeteriaServiceTerm { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public byte Status { get; set; }
    }
}
