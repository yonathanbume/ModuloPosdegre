using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class AcademicHistoryDocument : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }

        public string DocumentUrl { get; set; }

        public string PhysicalLocation { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }
    }
}
