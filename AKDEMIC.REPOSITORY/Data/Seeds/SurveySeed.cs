using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class SurveySeed
    {
        public static Survey[] Seed(AkdemicContext context)
        {
            var result = new List<Survey>()
            {
                new Survey
                {
                    Code = "COD1" ,
                    Description = "Encuesta ejemplo",
                    FinishDate = DateTime.ParseExact("31/07/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(),
                    Type = ConstantHelpers.TYPE_SURVEY.GENERAL,
                    Name = "Encuesta N°1",
                    PublicationDate = DateTime.ParseExact("01/07/2018", ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime()
                }
            };

            return result.ToArray();
        }
        
    }
}
