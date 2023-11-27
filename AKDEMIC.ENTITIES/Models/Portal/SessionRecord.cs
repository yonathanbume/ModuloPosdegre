using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class SessionRecord : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public bool OrderByFaculty { get; set; }
        public Guid? FacultyId { get; set; }
        public ICollection<SessionRecordFile> Files { get; set; }
    }
}
