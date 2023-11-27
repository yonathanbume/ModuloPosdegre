using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class WageItemSeed
    {
        public static WageItem[] Seed(AkdemicContext context)
        {
            var result = new List<WageItem>
            {
                //new WageItem { Name = "Haber Básico", Type = ConstantHelpers.PAYROLL.WAGE_ITEM_TYPES.INCOME, VariableType = ConstantHelpers.PAYROLL.WAGE_ITEM_VARIABLE_TYPES.STATIC }
            };

            return result.ToArray();
        }
    }
}
