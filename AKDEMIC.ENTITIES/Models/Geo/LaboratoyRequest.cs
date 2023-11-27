using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Geo
{
    public class LaboratoyRequest : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid SectionId { get; set; }

        public String TeacherId { get; set; }

        public byte State { get; set; }

        public String Response { get; set; }

        public DateTime DateResponse { get; set; }

        public String Description { get; set; }

        public Section Section { get; set; }

        public Teacher Teacher { get; set; }
    }
}
