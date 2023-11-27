using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamClassroomCareerRepository:IRepository<AdmissionExamClassroomCareer>
    {
        Task DeleteByAdmissionExamClassroomId(Guid id);
        Task<List<AdmissionExamClassroomCareer>> GetByAdmissionExamClassroomId(Guid id);
    }
}
