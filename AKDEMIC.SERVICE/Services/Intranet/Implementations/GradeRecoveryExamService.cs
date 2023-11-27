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
    public class GradeRecoveryExamService : IGradeRecoveryExamService
    {
        private readonly IGradeRecoveryExamRepository _gradeRecoveryDetailRepository;

        public GradeRecoveryExamService(IGradeRecoveryExamRepository gradeRecoveryDetailRepository)
        {
            _gradeRecoveryDetailRepository = gradeRecoveryDetailRepository;
        }

        public async Task<bool> AnyBySectionId(Guid sectionId)
            => await _gradeRecoveryDetailRepository.AnyBySection(sectionId);

        public async Task<GradeRecoveryExam> Get(Guid id)
            => await _gradeRecoveryDetailRepository.Get(id);

        public async Task<IEnumerable<GradeRecoveryExam>> GetGradeRecoveryByStudent(Guid studentId, Guid termId)
            => await _gradeRecoveryDetailRepository.GetGradeRecoveryByStudent(studentId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDetailDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, int? cycle, Guid? courseId, string searchValue)
            => await _gradeRecoveryDetailRepository.GetGradeRecoveryDetailDatatable(parameters, careerId, curriculumId, cycle, courseId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryExamByTeacherDatatable(DataTablesStructs.SentParameters parameters, byte? status, string teacherId)
            => await _gradeRecoveryDetailRepository.GetGradeRecoveryExamByTeacherDatatable(parameters, status, teacherId);

        public async Task Insert(GradeRecoveryExam entity)
            => await _gradeRecoveryDetailRepository.Insert(entity);

        public async Task Update(GradeRecoveryExam entity)
            => await _gradeRecoveryDetailRepository.Update(entity);
    }
}
