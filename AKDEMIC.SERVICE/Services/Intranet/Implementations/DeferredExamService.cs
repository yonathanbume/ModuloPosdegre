using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DeferredExamService : IDeferredExamService
    {
        private readonly IDeferredExamRepository _deferredExamRepository;

        public DeferredExamService(IDeferredExamRepository deferredExamRepository)
        {
            _deferredExamRepository = deferredExamRepository;
        }

        public async Task Insert(DeferredExam entity)
            => await _deferredExamRepository.Insert(entity);

        public async Task<bool> AnyBySection(Guid sectionId)
            => await _deferredExamRepository.AnyBySection(sectionId);

        public async Task Delete(DeferredExam entity)
            => await _deferredExamRepository.Delete(entity);

        public async Task<DeferredExam> Get(Guid id)
            => await _deferredExamRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null)
            => await _deferredExamRepository.GetDeferredExamDatatable(parameters, termId, careerId, curriculumId, academicYear, search, user);

        public async Task<List<StudentSection>> GetStudentSectionsAvailableToDeferredExam(Guid sectionId)
            => await _deferredExamRepository.GetStudentSectionsAvailableToDeferredExam(sectionId);

        public async Task Update(DeferredExam entity)
            => await _deferredExamRepository.Update(entity);
    }
}
