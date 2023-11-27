using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class DocumentTypeSeed
    {
        public static DocumentType[] Seed(AkdemicContext context)
        {
            var result = new List<DocumentType>()
            {
                new DocumentType { Name = "Administrativo", Code = "DA" },
                new DocumentType { Name = "Requisito", Code = "DR" }
            };

            return result.ToArray();
        }
    }
}
