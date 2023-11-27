using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VForumFile
    {
        public Guid Id { get; set; }
        public Guid VForumId { get; set; }
        public VForum VForum { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
