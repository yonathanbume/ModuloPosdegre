using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEvaluationRepository : IRepository<ENTITIES.Models.Enrollment.Evaluation>
    {
        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseUnit(Guid courseUnitId);
        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseTerm(Guid courseTermId);

        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTerm(Guid courseId, Guid termId);

        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTermWithTaken(Guid courseId, Guid termId, Guid sectionId);
        Task<object> GetAllAsModelA(Guid? termId = null, Guid? courseId = null);
        Task<object> GetAsModelB(Guid? id = null);
        Task<int> GenerateUnitsAndEvaluationsJob();
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId);
        Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetEvaluationsByClass(Guid classId);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatableByTeacherConfiguration(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId, Guid sectionId);
    }
}