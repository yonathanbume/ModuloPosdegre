using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureCategorySeed
    {
        public static ProcedureCategory[] Seed(AkdemicContext context)
        {
            var result = new List<ProcedureCategory>()
            {
                new ProcedureCategory { Name = ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES] , StaticType = ConstantHelpers.PROCEDURE_CATEGORIES.STATIC_TYPE.DEGREES_AND_TITLES },

                new ProcedureCategory { Name = "MATRÍCULA" },
                new ProcedureCategory { Name = "COMPLEMENTACION ACADEMICA Y PEDAGOGICA" },
                new ProcedureCategory { Name = "TARIFA DE ENSAYOS DE LABORATORIO DE MECANICA DE SUELOS FIC UNICA" },
                new ProcedureCategory { Name = "TALLER MECÁNICO" }
            };

            return result.ToArray();
        }
    }
}
