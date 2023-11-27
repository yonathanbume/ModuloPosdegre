using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AdmissionTypeDescountSeed
    {
        public static AdmissionTypeDescount[] Seed(AkdemicContext context)
        {
            var admissionTypes = context.AdmissionTypes.ToList();
            var terms = context.Terms.ToList();

            var result = new List<AdmissionTypeDescount>()
            {
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[0].Id, TermId = terms[0].Id, Descount = 0.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[1].Id, TermId = terms[0].Id, Descount = 10.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[2].Id, TermId = terms[0].Id, Descount = 20.00f },

                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[0].Id, TermId = terms[1].Id, Descount = 5.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[1].Id, TermId = terms[1].Id, Descount = 10.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[2].Id, TermId = terms[1].Id, Descount = 10.00f },

                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[0].Id, TermId = terms[2].Id, Descount = 5.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[1].Id, TermId = terms[2].Id, Descount = 10.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[2].Id, TermId = terms[2].Id, Descount = 10.00f },

                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[0].Id, TermId = terms[3].Id, Descount = 5.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[1].Id, TermId = terms[3].Id, Descount = 10.00f },
                new AdmissionTypeDescount { AdmissionTypeId = admissionTypes[2].Id, TermId = terms[3].Id, Descount = 10.00f }
            };

            return result.ToArray();
        }
    }
}
