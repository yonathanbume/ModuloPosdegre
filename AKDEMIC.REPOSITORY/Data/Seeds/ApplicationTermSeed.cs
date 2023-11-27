using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ApplicationTermSeed
    {
        public static ApplicationTerm[] Seed(AkdemicContext context)
        {
            var terms = context.Terms.ToList();

            var result = new List<ApplicationTerm>()
            {
                new ApplicationTerm { StartDate = DateTime.Parse("2017-04-01").ToUniversalTime(), EndDate = DateTime.Parse("2017-07-31").ToUniversalTime(), Status = 2, TermId = terms.First(x => x.Name == "2017-2").Id, InscriptionStartDate = DateTime.Parse("2017-04-01").ToUniversalTime(), InscriptionEndDate = DateTime.Parse("2017-07-31").ToUniversalTime(), PublicationDate = DateTime.Parse("2017-07-29").ToUniversalTime() },
                new ApplicationTerm { StartDate = DateTime.Parse("2017-10-22").ToUniversalTime(), EndDate = DateTime.Parse("2017-12-30").ToUniversalTime(), Status = 2, TermId = terms.First(x => x.Name == "2018-1").Id, InscriptionStartDate = DateTime.Parse("2017-10-22").ToUniversalTime(), InscriptionEndDate = DateTime.Parse("2017-12-30").ToUniversalTime(), PublicationDate = DateTime.Parse("2017-12-28").ToUniversalTime() },
                new ApplicationTerm { StartDate = DateTime.Parse("2018-04-01").ToUniversalTime(), EndDate = DateTime.Parse("2018-07-31").ToUniversalTime(), Status = 1, TermId = terms.First(x => x.Name == "2018-2").Id, InscriptionStartDate = DateTime.Parse("2018-04-01").ToUniversalTime(), InscriptionEndDate = DateTime.Parse("2018-07-31").ToUniversalTime(), PublicationDate = DateTime.Parse("2018-07-29").ToUniversalTime() }
            };

            return result.ToArray();

            //return new ApplicationTerm[] { };
        }
    }
}
