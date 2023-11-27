using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class DocumentaryRecordTypeSeed
    {
        public static DocumentaryRecordType[] Seed(AkdemicContext context)
        {
            var result = new List<DocumentaryRecordType>()
            {
                new DocumentaryRecordType { Name = "Tipo A", Code = "A" },
                new DocumentaryRecordType { Name = "Tipo B", Code = "B" },
                new DocumentaryRecordType { Name = "Tipo C", Code = "C" }
            };

            return result.ToArray();
        }
    }
}
