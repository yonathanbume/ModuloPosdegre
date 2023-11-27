using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationRepository : IRepository<ExtraordinaryEvaluation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, Guid? careerId, string teacherId, Guid? termId, ClaimsPrincipal user, bool? toEvalutionReport = null, byte? type = null);
        Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsToTeacherDatatable(DataTablesStructs.SentParameters parameters, string searchValue, string teacherId);
        Task<bool> AnyByCourseAndTermId(Guid courseId, Guid termId);
    }
}
