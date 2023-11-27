using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class EventSeed
    {
        public static Event[] Seed(AkdemicContext context)
        {
            var eventType = context.EventTypes.ToList();
            var organizer = context.Users.Where(x => x.UserName == "superadmin").ToList();

            var result = new List<Event>()
            {            
                new Event {
                    Name = "Example",
                    Cost = 20,
                    Description = "Example Event",
                    EventDate = DateTime.ParseExact("01/08/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(),
                    EventTypeId = eventType[0].Id,
                    OrganizerId = organizer[0].Id,
                    Place = "Lima",
                    RegistrationEndDate = DateTime.ParseExact("23/07/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(),
                    RegistrationStartDate = DateTime.ParseExact("20/07/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(),
                    PathPicture = "https://aval.com.ar/gestor/wp-content/uploads/2018/01/evento-Twitter.jpg"

                }
            };
            return result.ToArray();
        }
    }
}
