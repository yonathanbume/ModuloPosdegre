using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureSubcategorySeed
    {
        public static ProcedureSubcategory[] Seed(AkdemicContext context)
        {
            var procedureCategory = context.ProcedureCategories.ToList();

            var result = new List<ProcedureSubcategory>()
            {
                new ProcedureSubcategory { Name = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.BACHELOR], ProcedureCategoryId = procedureCategory[0].Id , StaticType = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.BACHELOR },
                new ProcedureSubcategory { Name = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.PROFESIONAL_TITLE], ProcedureCategoryId = procedureCategory[0].Id , StaticType = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.PROFESIONAL_TITLE },
                new ProcedureSubcategory { Name = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.MASTER], ProcedureCategoryId = procedureCategory[0].Id , StaticType = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.MASTER },
                new ProcedureSubcategory { Name = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.DOCTOR], ProcedureCategoryId = procedureCategory[0].Id , StaticType = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.DOCTOR },
                new ProcedureSubcategory { Name = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.SECOND_SPECIALTY_DEGREE], ProcedureCategoryId = procedureCategory[4].Id , StaticType = ConstantHelpers.PROCEDURE_SUBCATEGORIES.STATIC_TYPE.SECOND_SPECIALTY_DEGREE },

                new ProcedureSubcategory { Name = "LICENCIATURA EN CIENCIAS DE LA EDUCACIÓN", ProcedureCategoryId = procedureCategory[0].Id },
                new ProcedureSubcategory { Name = "ESTANDARES DE SUELO", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "ESPECIALES PARA CIMENTACIONES", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "ENSAYOS DE CALIDAD DE AGREGADO", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "MEZCLAS BITUMINOSAS", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "PRUEBA IN SITU", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "CONCRETO", ProcedureCategoryId = procedureCategory[1].Id },
                new ProcedureSubcategory { Name = "ALBAÑILERÍA", ProcedureCategoryId = procedureCategory[1].Id }
                
            };

            return result.ToArray();
        }
    }
}
