using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ForumCareer
    {
        public Guid Id { get; set; }

        public Guid ForumId { get; set; }
        public Forum Forum { get; set; }

        public Guid CareerId { get; set; }
        public Career Career { get; set; }
    }
}