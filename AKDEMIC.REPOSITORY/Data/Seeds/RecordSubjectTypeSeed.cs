using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class RecordSubjectTypeSeed
    {
        public static RecordSubjectType[] Seed(AkdemicContext context)
        {
            var result = new List<RecordSubjectType>()
            {
                new RecordSubjectType { Name = "Asunto Simple", Code = "AS" },
                new RecordSubjectType { Name = "Asunto Urgente", Code = "AU" }
            };

            return result.ToArray();
        }
    }
}
