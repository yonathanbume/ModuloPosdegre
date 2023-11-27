using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ExternalProcedureSeed
    {
        public static ExternalProcedure[] Seed(AkdemicContext context)
        {
            var classifiers = context.Classifiers.ToList();
            var dependencies = context.Dependencies.AsQueryable();

            var result = new List<ExternalProcedure>()
            {
                new ExternalProcedure { ClassifierId = null, DependencyId = dependencies.Where(x => x.Name == "Mesa de Partes").First().Id, Code = "TE-LIBREC001", Comment = "", Cost = 0.00M, Name = "Libro de Reclamaciones", StaticType = ConstantHelpers.INTERNAL_PROCEDURES.STATIC_TYPE.COMPLAINTS_BOOK },
                new ExternalProcedure { ClassifierId = classifiers[4].Id, DependencyId = dependencies.Where(x => x.Name == "Biblioteca").First().Id, Code = "0001", Comment = "", Cost = 10.50M, Name = "Trámite Externo 1" },
                new ExternalProcedure { ClassifierId = classifiers[3].Id, DependencyId = dependencies.Where(x => x.Name == "Centro Médico").First().Id, Code = "0002", Comment = "", Cost = 20.50M, Name = "Trámite Externo 2" },
                new ExternalProcedure { ClassifierId = classifiers[2].Id, DependencyId = dependencies.Where(x => x.Name == "Histórico").First().Id, Code = "0003", Comment = "", Cost = 30.50M, Name = "Trámite Externo 3" },
                new ExternalProcedure { ClassifierId = classifiers[1].Id, DependencyId = dependencies.Where(x => x.Name == "Mesa de Partes").First().Id, Code = "0004", Comment = "", Cost = 40.50M, Name = "Trámite Externo 4" },
                new ExternalProcedure { ClassifierId = classifiers[0].Id, DependencyId = dependencies.Where(x => x.Name == "Oficina").First().Id, Code = "0005", Comment = "", Cost = 50.50M, Name = "Trámite Externo 5" }
            };

            return result.ToArray();
        }
    }
}
