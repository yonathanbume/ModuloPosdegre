using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public sealed class SubstituteExamService : ISubstituteExamService
    {
        private readonly ISubstituteExamRepository _substituteExamRepository;
        public SubstituteExamService(ISubstituteExamRepository substituteExamRepository)
        {
            _substituteExamRepository = substituteExamRepository;
        }

        Task<DataTablesStructs.ReturnedData<object>> ISubstituteExamService.GetDatatableByFilters(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, Guid? courseId = null, Guid? sectionId = null, byte? status = null)
            => _substituteExamRepository.GetDatatableByFilters(sentParameters, teacherId, termId, courseId, sectionId, status);

        Task<bool> ISubstituteExamService.AnyByCourseIdTermIdAndStudentId(Guid courseId, Guid termId, Guid studentId, Guid? id = null)
            => _substituteExamRepository.AnyByCourseIdTermIdAndStudentId(courseId, termId, studentId, id);

        Task ISubstituteExamService.DeleteAsync(SubstituteExam substituteExam)
            => _substituteExamRepository.Delete(substituteExam);

        Task<DataTablesStructs.ReturnedData<object>> ISubstituteExamService.GetAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId)
            => _substituteExamRepository.GetAcademicHistoryDatatable(sentParameters, termId);

        Task<object> ISubstituteExamService.GetAsModelA(Guid? id)
            => _substituteExamRepository.GetAsModelA(id);

        Task<SubstituteExam> ISubstituteExamService.GetAsync(Guid id)
            => _substituteExamRepository.Get(id);

        Task ISubstituteExamService.InsertAsync(SubstituteExam substituteExam)
            => _substituteExamRepository.Insert(substituteExam);

        Task ISubstituteExamService.UpdateAsync(SubstituteExam substituteExam)
            => _substituteExamRepository.Update(substituteExam);

        public async Task<SubstituteExam> GetSubstituteExamByStudentId(Guid studentId)
            => await _substituteExamRepository.GetSubstituteExamByStudentId(studentId);

        public async Task DeleteAllByCourseTerm(Guid courseTermId)
        {
            await _substituteExamRepository.DeleteAllByCourseTerm(courseTermId);
        }

        public async Task<int?> GetExamScoreByCourseAndTermAndStudent(Guid courseId, Guid termId, Guid studentId)
            => await _substituteExamRepository.GetExamScoreByCourseAndTermAndStudent(courseId, termId, studentId);

        public async Task InsertRangeAsync(IEnumerable<SubstituteExam> substituteExam)
        {
            await _substituteExamRepository.InsertRange(substituteExam);
        }

        public async Task SaveStudentsForSubstituteExam(Guid termid, Guid sectionId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            await _substituteExamRepository.SaveStudentsForSubstituteExam(termid, sectionId, isCheckAll, lstToAdd, lstToAvoid);
        }

        public async Task<bool> AnyByCourseTermIdAndStudentId(Guid courseTermId, Guid id)
        {
            return await _substituteExamRepository.AnyByCourseTermIdAndStudentId(courseTermId, id);
        }

        public async Task<bool> ChangeCourseTermIdToSectionId()
        {
           return await _substituteExamRepository.ChangeCourseTermIdToSectionId();
        }
        public async Task<bool> AnyBySectionId(Guid sectionId)
        {
            return await _substituteExamRepository.AnyBySectionId(sectionId);
        }

        public async Task<SubstituteExam> GetSubstituteExamByStudentAndSectionId(Guid studentId, Guid sectionId)
            => await _substituteExamRepository.GetSubstituteExamByStudentAndSectionId(studentId, sectionId);
        public async Task<SubstituteExam> GetSubstituteExamByStudentIdAndCourse(Guid studentId, Guid courseId)
            => await _substituteExamRepository.GetSubstituteExamByStudentIdAndCourse(studentId, courseId);

        public async Task<bool> AnySubstituteExamByStudent(Guid studentId, Guid sectionId, byte status)
            => await _substituteExamRepository.AnySubstituteExamByStudent(studentId, sectionId, status);
    }
}