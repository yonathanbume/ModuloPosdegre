using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAcademicYearCreditRepository : IRepository<AcademicYearCredit>
    {
        //Task<AcademicYearCredit> Get(Guid curriculumId, int academicYear);
        Task<List<AcademicYearCredit>> GetCurriculumAcademicYearCredits(Guid curriculumId);
    }
}
