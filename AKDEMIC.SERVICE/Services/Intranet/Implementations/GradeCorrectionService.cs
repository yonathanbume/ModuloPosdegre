using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeCorrectionService : IGradeCorrectionService
    {
        private readonly IGradeCorrectionRepository _gradeCorrectionRepository;

        public GradeCorrectionService(IGradeCorrectionRepository gradeCorrectionRepository)
        {
            _gradeCorrectionRepository = gradeCorrectionRepository;
        }

        public async Task<bool> AnyGradeCorrectionByFilters(Guid gradeId, int status)
            => await _gradeCorrectionRepository.AnyGradeCorrectionByFilters(gradeId, status);

        public Task DeleteById(Guid id)
            => _gradeCorrectionRepository.DeleteById(id);

        public Task<GradeCorrection> Get(Guid id)
            => _gradeCorrectionRepository.Get(id);

        public Task<IEnumerable<GradeCorrection>> GetAll(string teacherId = null, Guid? termId = null)
            => _gradeCorrectionRepository.GetAll(teacherId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByRoleDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, int? state = null, ClaimsPrincipal user = null)
            => await _gradeCorrectionRepository.GetAllByRoleDatatable(sentParameters, termId, searchValue, state, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
            => await _gradeCorrectionRepository.GetAllDatatable(sentParameters, teacherId, termId, searchValue, state);

        public async Task<GradeCorrection> GetByTeacherStudent(string teacherId, Guid studentId)
            => await _gradeCorrectionRepository.GetByTeacherStudent(teacherId, studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeCorrectionsRequestedByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? studentId, string search)
            => await _gradeCorrectionRepository.GetGradeCorrectionsRequestedByStudentDatatable(sentParameters, termId, studentId, search);

        public Task Insert(GradeCorrection gradeCorrection)
            => _gradeCorrectionRepository.Insert(gradeCorrection);

        public Task Update(GradeCorrection gradeCorrection)
            => _gradeCorrectionRepository.Update(gradeCorrection);
    }
}
