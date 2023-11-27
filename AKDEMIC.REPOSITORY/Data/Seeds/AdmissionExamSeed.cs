using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AdmissionExamSeed
    {
        public static AdmissionExam[] Seed(AkdemicContext context)
        {            
            var result = new List<AdmissionExam>()
            {
                new AdmissionExam { Code = "C001", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C002", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C003", Name = "Examen de Admision" },

                new AdmissionExam { Code = "C004", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C005", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C006", Name = "Examen de Admision" },

                new AdmissionExam { Code = "C007", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C008", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C009", Name = "Examen de Admision" },

                new AdmissionExam { Code = "C010", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C011", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C012", Name = "Examen de Admision" },

                new AdmissionExam { Code = "C013", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C014", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C015", Name = "Examen de Admision" },

                new AdmissionExam { Code = "C016", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C017", Name = "Examen de Admision" },
                new AdmissionExam { Code = "C018", Name = "Examen de Admision" }
            };

            return result.ToArray();
        }
    }   
}

