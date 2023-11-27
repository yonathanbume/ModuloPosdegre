using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AdmissionResultSeed
    {
        public static AdmissionResult[] Seed(AkdemicContext context)
        {
            var postulants = context.Postulants.ToList();

            var result = new List<AdmissionResult>()
            {
                //new AdmissionResult() { Accepted = true, Grade = 20, Postulant = postulants[0] },
                //new AdmissionResult() { Accepted = false, Grade = 13, Postulant = postulants[1] },
                //new AdmissionResult() { Accepted = true, Grade = 16, Postulant = postulants[2] }
            };

            return result.ToArray();
        }
    }
}
