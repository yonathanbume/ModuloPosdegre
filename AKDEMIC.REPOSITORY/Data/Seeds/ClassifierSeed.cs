using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class ClassifierSeed
    {
        public static Classifier[] Seed(AkdemicContext context)
        {
            var result = new List<Classifier>()
            {
                new Classifier { Code = "HIS001", Description = "CL-HIS001", Name = "Histórico", RelationId = "HIS001" },
                new Classifier { Code = "C001", Description = "CL-0001", Name = "Clasificador 1", RelationId = "C001" },
                new Classifier { Code = "C002", Description = "CL-0002", Name = "Clasificador 2", RelationId = "C002" },
                new Classifier { Code = "C003", Description = "CL-0003", Name = "Clasificador 3", RelationId = "C003" },
                new Classifier { Code = "C004", Description = "CL-0004", Name = "Clasificador 4", RelationId = "C004" },
                new Classifier { Code = "C005", Description = "CL-0005", Name = "Clasificador 5", RelationId = "C005" },
                new Classifier { Code = "C006", Description = "CL-0006", Name = "Clasificador 6", RelationId = "C006" },
                new Classifier { Code = "C007", Description = "CL-0007", Name = "Clasificador 7", RelationId = "C007" },
                new Classifier { Code = "C008", Description = "CL-0008", Name = "Clasificador 8", RelationId = "C008" }
            };

            return result.ToArray();
        }
    }
}
