using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class InternalProcedureFileSeed
    {
        public static InternalProcedureFile[] Seed(AkdemicContext context)
        {
            var internalProcedures = context.InternalProcedures.ToList();
            var result = new List<InternalProcedureFile>();

            for (var i = 0; i < internalProcedures.Count; i++)
            {
                var internalProcedure = internalProcedures[i];

                result.AddRange(new InternalProcedureFile[]
                {
                    new InternalProcedureFile { InternalProcedureId = internalProcedure.Id, FileName = "Campus%20Agreement%20Datasheet.doc", Path = "http://download.microsoft.com/download/0/8/2/0821aeb8-8421-4635-9edb-2b5f1836f633/Campus%20Agreement%20Datasheet.doc", Size = 0 },
                    new InternalProcedureFile { InternalProcedureId = internalProcedure.Id, FileName = "DS-Deployment-Summary-English.xls", Path = "http://download.microsoft.com/documents/nl-nl/business/sam-inventarisatie/DS-Deployment-Summary-English.xls", Size = 0 },
                    new InternalProcedureFile { InternalProcedureId = internalProcedure.Id, FileName = "dotnet-bot_branded.png", Path = "https://raw.githubusercontent.com/dotnet/swag/71f7cbc4b74444d9022638f60ff03ca30e2a1ea6/dotnet-bot/dotnet-bot_branded.png", Size = 0 },
                    new InternalProcedureFile { InternalProcedureId = internalProcedure.Id, FileName = "Learn_Azure_in_a_Month_of_Lunches.pdf", Path = "https://azure.microsoft.com/mediahandler/files/resourcefiles/learn-azure-in-a-month-of-lunches/Learn_Azure_in_a_Month_of_Lunches.pdf", Size = 0 }
                });
            }

            return result.ToArray();
        }
    }
}
