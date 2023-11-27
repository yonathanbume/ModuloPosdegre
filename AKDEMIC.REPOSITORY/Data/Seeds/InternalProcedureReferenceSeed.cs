using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class InternalProcedureReferenceSeed
    {
        public static InternalProcedureReference[] Seed(AkdemicContext context)
        {
            var internalProcedures = context.InternalProcedures.ToList();
            var result = new List<InternalProcedureReference>();

            for (var i = 0; i < internalProcedures.Count; i++)
            {
                var internalProcedure = internalProcedures[i];

                result.AddRange(new InternalProcedureReference[]
                {
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 1" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 2" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 3" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 4" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 5" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 6" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 7" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 8" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 9" },
                    new InternalProcedureReference { InternalProcedureId = internalProcedure.Id, Reference = "Referencia 10" }
                });
            }

            return result.ToArray();
        }
    }
}
