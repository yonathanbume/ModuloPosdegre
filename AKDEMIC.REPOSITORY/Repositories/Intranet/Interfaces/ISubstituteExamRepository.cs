using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISubstituteExamRepository : IRepository<SubstituteExam>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatableByFilters(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, Guid? courseId = null, Guid? sectionId = null, byte? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null);
        Task<object> GetAsModelA(Guid? id = null);
        Task<bool> AnyByCourseIdTermIdAndStudentId(Guid courseId, Guid termId, Guid studentId, Guid? id = null);
        Task<bool> AnyByCourseTermIdAndStudentId(Guid courseTermId, Guid studentId);
        Task<SubstituteExam> GetSubstituteExamByStudentId(Guid studentId);
        Task DeleteAllByCourseTerm(Guid courseTermId);
        Task<int?> GetExamScoreByCourseAndTermAndStudent(Guid courseId, Guid termId, Guid studentId);
        Task SaveStudentsForSubstituteExam(Guid termid, Guid sectionId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid);
        Task<bool> ChangeCourseTermIdToSectionId();
        Task<bool> AnyBySectionId(Guid sectionId);
        Task<SubstituteExam> GetSubstituteExamByStudentAndSectionId(Guid studentId, Guid sectionId);
        Task<SubstituteExam> GetSubstituteExamByStudentIdAndCourse(Guid studentId, Guid courseId);
        Task<bool> AnySubstituteExamByStudent(Guid studentId, Guid sectionId, byte status);
    }
}