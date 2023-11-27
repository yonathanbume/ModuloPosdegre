using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AcademicDepartmentSeed
    {
        public static AcademicDepartment[] Seed(AkdemicContext context)
        {
            var faculties = context.Faculties.ToList();

            var result = new List<AcademicDepartment>()
            {
                new AcademicDepartment { Name = "Medicina", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Cirugía", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Ciencias fisiológicas", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Ciencias morfológicas", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Medicina social y de la conducta", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Materno infantel", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[5].Id },
                new AcademicDepartment { Name = "Ingeniería", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[0].Id },
                new AcademicDepartment { Name = "Ingeniería Civil", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[3].Id },
                new AcademicDepartment { Name = "Administración", Status = ConstantHelpers.STATES.ACTIVE, FacultyId = faculties[1].Id }
            };

            return result.ToArray();
        }
    }
}
