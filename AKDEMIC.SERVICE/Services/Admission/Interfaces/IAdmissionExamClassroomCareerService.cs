using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamClassroomCareerService
    {
        Task DeleteByAdmissionExamClassroomId(Guid id);
        Task<List<AdmissionExamClassroomCareer>> GetByAdmissionExamClassroomId(Guid id);
        Task InsertRange(List<AdmissionExamClassroomCareer> newTeachers);
    }
}
