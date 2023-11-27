using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class EventTypeSeed
    {
        public static EventType[] Seed(AkdemicContext context)
        {
            var result = new List<EventType>()
            {
                new EventType { Name = "Random" }
            };

            return result.ToArray();
        }
    }
}
