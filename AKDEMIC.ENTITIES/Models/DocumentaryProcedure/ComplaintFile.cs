using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ComplaintFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        public string FileUrl { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; } = 1; //Response
    }
}
