using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AnnouncementSeed
    {
        public static Announcement[] Seed(AkdemicContext context)
        {
            var result = new List<Announcement>()
            {
                new Announcement {
                    Title = "BIENVENIDOS",
                    Description ="Somos una institución académica científica de primer nivel en la región. Proclive al cambio y que contribuya activamente al desarrollo regional.",
                    Pathfile ="https://endimages.s3.amazonaws.com/cache/87/20/87201ff60140076d5916515587100e63.jpg",
                    RegisterDate = DateTime.UtcNow,
                    StartDate = DateTime.ParseExact("01/07/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(),
                    EndDate = DateTime.ParseExact("31/12/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime()
                }
            };

            return result.ToArray();
        }
    }
}
