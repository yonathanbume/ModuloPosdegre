using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CurriculumSeed
    {
        public static Curriculum[] Seed(AkdemicContext context)
        {
            var careers = context.Careers.ToList();

            var result = new List<Curriculum>()
            {
                new Curriculum { CareerId = careers[0].Id, Year = 2016, ValidityStart = DateTime.Parse("2016-01-01"), ValidityEnd = DateTime.Parse("2017-12-31"), StudyRegime = 1, CreationResolutionNumber = "RES. N° 175213", CreationResolutionDate = DateTime.Parse("2016-01-01"), CreationResolutionFile = "", IsActive = false, IsNew = false},
                new Curriculum { CareerId = careers[0].Id, Year = 2018, ValidityStart = DateTime.Parse("2018-01-01"), ValidityEnd = DateTime.Parse("2020-12-31"), StudyRegime = 1, CreationResolutionNumber = "RES. N° 175227", CreationResolutionDate = DateTime.Parse("2018-01-01"), CreationResolutionFile = "", IsActive = true, IsNew = false},
            };

            return result.ToArray();
        }
    }
}
