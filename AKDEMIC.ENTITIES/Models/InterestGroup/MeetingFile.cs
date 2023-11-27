using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class MeetingFile
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public string UrlFile { get; set; }
    }
}
