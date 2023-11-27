using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class SessionRecordFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid? FacultyId { get; set; }
        public bool IsLink { get; set; }
        public Guid SessionRecordId { get; set; }
        public Faculty Faculty { get; set; }
        public SessionRecord SessionRecord { get; set; }
    }
}
