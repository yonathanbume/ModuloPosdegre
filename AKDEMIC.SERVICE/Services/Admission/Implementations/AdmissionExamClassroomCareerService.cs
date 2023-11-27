using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamClassroomCareerService:IAdmissionExamClassroomCareerService
    {
        private readonly IAdmissionExamClassroomCareerRepository _admissionExamClassroomCareerRepository;
        public AdmissionExamClassroomCareerService(IAdmissionExamClassroomCareerRepository admissionExamClassroomCareerRepository)
        {
            _admissionExamClassroomCareerRepository = admissionExamClassroomCareerRepository;
        }

        public async Task DeleteByAdmissionExamClassroomId(Guid id)
        {
            await _admissionExamClassroomCareerRepository.DeleteByAdmissionExamClassroomId(id);
        }

        public async Task<List<AdmissionExamClassroomCareer>> GetByAdmissionExamClassroomId(Guid id)
        {
            return await _admissionExamClassroomCareerRepository.GetByAdmissionExamClassroomId(id);
        }

        public async Task InsertRange(List<AdmissionExamClassroomCareer> newTeachers)
        {
            await _admissionExamClassroomCareerRepository.InsertRange(newTeachers);
        }
    }
}
