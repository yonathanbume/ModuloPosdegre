using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class UITSeed
    {
        public static UIT[] Seed(AkdemicContext context)
        {
            var result = new List<UIT>()
            {
                new UIT { Value = 3850, Year = 2015 },
                new UIT { Value = 3950, Year = 2016 },
                new UIT { Value = 4050, Year = 2017 },
                new UIT { Value = 4150, Year = 2018 }
            };

            return result.ToArray();
        }
    }
}
