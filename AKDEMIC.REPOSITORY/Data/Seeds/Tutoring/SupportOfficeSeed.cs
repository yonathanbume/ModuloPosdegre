using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class SupportOfficeSeed
    {
        public static SupportOffice[] Seed(AkdemicContext context)
        {
            var result = new List<SupportOffice>()
            {
                new SupportOffice { Name = "Psicología" },
                new SupportOffice { Name = "Educación" }
            };
            return result.ToArray();
        }
    }
}
