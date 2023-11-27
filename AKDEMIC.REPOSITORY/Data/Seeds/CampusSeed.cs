using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CampusSeed
    {
        public static Campus[] Seed(AkdemicContext apiContext)
        {
            Guid districtId = apiContext.Districts.Where(x => x.Name == "Lima").First().Id;
            List<Campus> result = new List<Campus>()
            {
                new Campus { Code = "S01", IsValid = true, DistrictId = districtId, Name = "Sede Lima" , IsPrincipal = true , Address = "Manuel Olguin 335" },
                new Campus { Code = "S02", IsValid = true, DistrictId = districtId, Name = "Sede Pueblo Libre" },
                new Campus { Code = "S03", IsValid = true, DistrictId = districtId, Name = "Sede Los Olivos" }
            };
            return result.ToArray();
        }
    }
}
