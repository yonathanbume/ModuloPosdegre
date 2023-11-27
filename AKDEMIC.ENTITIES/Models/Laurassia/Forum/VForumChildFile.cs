using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VForumChildFile
    {
        public Guid Id { get; set; }
        public Guid VForumChildId { get; set; }
        public VForumChild VForumChild { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
