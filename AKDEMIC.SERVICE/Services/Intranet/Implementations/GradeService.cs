using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public Task<int> CountByFilter(Guid? evaluationId = null)
            => _gradeRepository.CountByFilter(evaluationId);

        public Task DeleteById(Grade id)
            => _gradeRepository.DeleteById(id);

        public Task<Grade> Get(Guid id)
            => _gradeRepository.Get(id);

        public Task<IEnumerable<Grade>> GetAll()
            => _gradeRepository.GetAll();

        public Task<IEnumerable<Grade>> GetAll(Guid? studentSectionId = null, Guid? studentId = null, Guid? sectionId = null)
            => _gradeRepository.GetAll(studentSectionId, studentId, sectionId);

        public Task Insert(Grade grade)
            => _gradeRepository.Insert(grade);

        public Task Update(Grade grade)
            => _gradeRepository.Update(grade);

        public async Task<List<Grade>> GetGradesByStudentSectionId(Guid studentSectionId)
        {
            return await _gradeRepository.GetGradesByStudentSectionId(studentSectionId);
        }

        public async Task<List<Grade>> GetGradesBySectionId(Guid sectionId)
            => await _gradeRepository.GetGradesBySectionId(sectionId);
        public async Task<Select2Structs.ResponseParameters> GetStudentsBySectionAndEvaluation(Select2Structs.RequestParameters requestParameters, Guid sectionId, Guid? evaluationId = null, string searchValue = null)
            => await _gradeRepository.GetStudentsBySectionAndEvaluation(requestParameters, sectionId, evaluationId, searchValue);

        public async Task<object> GetStudentGradesDatatable(Guid studentSectionId)
            => await _gradeRepository.GetStudentGradesDatatable(studentSectionId);

        public async Task<List<Grade>> GetGradesByStudentAndTerm(Guid studentId, Guid termId)
            => await _gradeRepository.GetGradesByStudentAndTerm(studentId, termId);

    }
}