using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class ChannelCareer
    {
        public Guid CareerId { get; set; }
        public Guid ChannelId { get; set; }

        public Career Career { get; set; }
        public Channel Channel { get; set; }
    }
}
