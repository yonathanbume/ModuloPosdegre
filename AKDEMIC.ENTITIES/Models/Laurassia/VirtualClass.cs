using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VirtualClass : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string JoinUrl { get; set; }
        public string RegisterUrl { get; set; } //delete later google
        public byte Integration { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
        public bool Show { get; set; } = true;
        public Guid? VirtualClassCredentialId { get; set; }
        public VirtualClassCredential VirtualClassCredential { get; set; }
        public Guid? VirtualClassDetailId { get; set; }
        public VirtualClassDetail VirtualClassDetail { get; set; }
        public List<VirtualClassRecording> VirtualClassRecordings { get; set; }
        public List<VirtualClassLog> VirtualClassLogs { get; set; }
    }
}
