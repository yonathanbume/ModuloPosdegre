using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VirtualClassRecording : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VirtualClassId { get; set; }
        public VirtualClass VirtualClass { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string PlayUrl { get; set; }
        public string DownloadUrl { get; set; }
    }
}
