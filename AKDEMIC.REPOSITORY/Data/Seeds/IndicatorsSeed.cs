using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Indicators;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class IndicatorsSeed
    {
        public static Indicators[] Seed(AkdemicContext _context)
        {
            var result = new List<Indicators>()
            {
                new Indicators{ Title = "Indicador de Faltas de Docentes", MindRank = 0,MediumdRank = 10,MaxdRank = 20 },
                new Indicators{ Title = "Indicador de Demora en Trámites", MindRank = 0,MediumdRank = 10,MaxdRank = 20 }
            };
            return result.ToArray();
        }
    }
}
