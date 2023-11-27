using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Laurassia;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ManualSeed
    {
        public static Manual[] Seed(AkdemicContext context)
        {
            var result = new List<Manual>()
            {
                new Manual {
                Title = "Manual de aula virtual",
                Content = "Manual para entender mejor el aula virtual",
                Type = ConstantHelpers.ROLES.STUDENTS,
                URL = "https://akdemic.blob.core.windows.net/manual/1bcd9451-cde2-49bf-8084-69d845118f40.pdf" },
                new Manual {
                Title = "Manual de Web Tour",
                Content = "Manual para poder manejar mejor el Web Tour",
                Type = ConstantHelpers.ROLES.TEACHERS,
                URL = "https://akdemic.blob.core.windows.net/manual/1bcd9451-cde2-49bf-8084-69d845118f40.pdf" }
            };

            return result.ToArray();
        }
    }
}
