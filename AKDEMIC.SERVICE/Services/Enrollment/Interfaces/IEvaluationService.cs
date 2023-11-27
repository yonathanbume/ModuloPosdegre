using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEvaluationService
    {
        Task<ENTITIES.Models.Enrollment.Evaluation> Get(Guid id);

        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseTerm(Guid courseTermId);
        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseUnit(Guid courseUnitId);

        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTerm(Guid courseId, Guid termId);

        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTermWithTaken(Guid courseId, Guid termId, Guid sectionId);

        Task InsertAsync(ENTITIES.Models.Enrollment.Evaluation evaluation);
        Task InsertRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations);
        Task UpdateAsync(ENTITIES.Models.Enrollment.Evaluation evaluation);
        Task UpdateRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations);
        Task DeleteAsync(ENTITIES.Models.Enrollment.Evaluation evaluation);
        Task DeleteRangeAsync(IEnumerable<ENTITIES.Models.Enrollment.Evaluation> evaluations);
        Task<object> GetAllAsModelA(Guid? termId = null, Guid? courseId = null);
        Task<object> GetAsModelB(Guid? id = null);
        Task DeleteById(Guid id);
        Task<int> GenerateUnitsAndEvaluationsJob();
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId);
        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetEvaluationsByClass(Guid classId);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatableByTeacherConfiguration(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId, Guid sectionId);
    }
}