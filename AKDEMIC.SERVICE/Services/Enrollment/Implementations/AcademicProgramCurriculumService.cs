using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class AcademicProgramCurriculumService : IAcademicProgramCurriculumService
    {
        private readonly IAcademicProgramCurriculumRepository _academicProgramCurriculumRepository;

        public AcademicProgramCurriculumService(IAcademicProgramCurriculumRepository academicProgramCurriculumRepository)
        {
            _academicProgramCurriculumRepository = academicProgramCurriculumRepository;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetByAcademicProgramIdSelect2ClientSide(Guid careerId, Guid academicProgramId)
        {
            return await _academicProgramCurriculumRepository.GetByAcademicProgramIdSelect2ClientSide(careerId,academicProgramId);
        }

        public async Task LoadAcademicProgramsJob()
        {
            await _academicProgramCurriculumRepository.LoadAcademicProgramsJob();
        }

        Task<bool> IAcademicProgramCurriculumService.AnyByAcademicProgramAndCurriculum(Guid academicProgramId, Guid curriculumId)
            => _academicProgramCurriculumRepository.AnyByAcademicProgramAndCurriculum(academicProgramId, curriculumId);

        Task IAcademicProgramCurriculumService.DeleteAsync(AcademicProgramCurriculum academicProgramCurriculum)
            => _academicProgramCurriculumRepository.Delete(academicProgramCurriculum);

        Task<AcademicProgramCurriculum> IAcademicProgramCurriculumService.GetAsync(Guid id)
            => _academicProgramCurriculumRepository.Get(id);

        Task<AcademicProgramCurriculum> IAcademicProgramCurriculumService.GetByFilter(Guid academicProgramId, Guid curriculumId)
            => _academicProgramCurriculumRepository.GetByFilter(academicProgramId, curriculumId);

        Task IAcademicProgramCurriculumService.InsertAsync(AcademicProgramCurriculum academicProgramCurriculum)
            => _academicProgramCurriculumRepository.Insert(academicProgramCurriculum);

        Task IAcademicProgramCurriculumService.UpdateAsync(AcademicProgramCurriculum academicProgramCurriculum)
            => _academicProgramCurriculumRepository.Update(academicProgramCurriculum);
    }
}