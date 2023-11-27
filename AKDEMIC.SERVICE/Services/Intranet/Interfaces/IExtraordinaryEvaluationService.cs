using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationService
    {
        Task Insert(ExtraordinaryEvaluation extraordinaryEvaluation);
        Task<ExtraordinaryEvaluation> Get(Guid id);
        Task Delete(ExtraordinaryEvaluation entity);
        Task Update(ExtraordinaryEvaluation extraordinaryEvaluation);
        Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, Guid? careerId, string teacherId, Guid? termId, ClaimsPrincipal user,bool? toEvaluationReport = null, byte? type = null);
        Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsToTeacherDatatable(DataTablesStructs.SentParameters parameters, string searchValue, string teacherId);
        Task<bool> AnyByCourseAndTermId(Guid courseId, Guid termId);
    }
}
