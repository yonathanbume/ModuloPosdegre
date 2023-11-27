using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeRectificationService : IGradeRectificationService
    {
        private readonly IGradeRectificationRepository _gradeRectificationRepository;

        public GradeRectificationService(IGradeRectificationRepository gradeRectificationRepository)
        {
            _gradeRectificationRepository = gradeRectificationRepository;
        }
        public Task Insert(GradeRectification gradeCorrection)
            => _gradeRectificationRepository.Insert(gradeCorrection);
        public Task InsertRange(List<GradeRectification> gradeCorrections)
            => _gradeRectificationRepository.InsertRange(gradeCorrections);
        public Task Update(GradeRectification gradeCorrection)
            => _gradeRectificationRepository.Update(gradeCorrection);
        public Task DeleteById(Guid id)
            => _gradeRectificationRepository.DeleteById(id);

        public Task<GradeRectification> Get(Guid id)
            => _gradeRectificationRepository.Get(id);

        public Task<IEnumerable<GradeRectification>> GetAll(string teacherId = null, Guid? termId = null)
            => _gradeRectificationRepository.GetAll(teacherId, termId);
        public async Task<bool> AnySubstituteexams(Guid studentId, Guid courseId)
            => await _gradeRectificationRepository.AnySubstituteexams(studentId, courseId);
        public async Task<bool> AnyByEvaluation(Guid evaluationId)
            => await _gradeRectificationRepository.AnyByEvaluation(evaluationId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
            => await _gradeRectificationRepository.GetAllDatatable(sentParameters, teacherId, termId, searchValue, state);
    }
}
