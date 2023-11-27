using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class FacultyCurriculumAreaSeed
    {
        public static FacultyCurriculumArea[] Seed(AkdemicContext apiContext)
        {
            var faculties = apiContext.Faculties.ToArray();
            var areas = apiContext.CurriculumAreas.ToArray();

            var result = new List<FacultyCurriculumArea>
            {
                new FacultyCurriculumArea { FacultyId = faculties[0].Id, CurriculumAreaId = areas[0].Id },
                new FacultyCurriculumArea { FacultyId = faculties[3].Id, CurriculumAreaId = areas[0].Id },
                new FacultyCurriculumArea { FacultyId = faculties[4].Id, CurriculumAreaId = areas[0].Id },
                new FacultyCurriculumArea { FacultyId = faculties[5].Id, CurriculumAreaId = areas[0].Id },
                new FacultyCurriculumArea { FacultyId = faculties[1].Id, CurriculumAreaId = areas[1].Id },
                new FacultyCurriculumArea { FacultyId = faculties[2].Id, CurriculumAreaId = areas[1].Id }
            };  

            return result.ToArray();
        }
    }
}
