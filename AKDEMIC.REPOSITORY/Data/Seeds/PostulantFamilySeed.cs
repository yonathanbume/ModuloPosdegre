using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PostulantFamilySeed
    {
        public static PostulantFamily[] Seed(AkdemicContext context)
        {
            var result = new List<PostulantFamily>()
            {
            };

            return result.ToArray();
        }
    }
}
