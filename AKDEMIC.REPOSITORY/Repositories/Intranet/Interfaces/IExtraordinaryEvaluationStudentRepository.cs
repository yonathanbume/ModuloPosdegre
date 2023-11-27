using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationStudentRepository : IRepository<ExtraordinaryEvaluationStudent>
    {
        Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationId(Guid extraordinaryEvaluationId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid extraordinaryEvaluationId, string searchValue);
        Task<ExtraordinaryEvaluationReportTemplate> GetEvaluationReportInformation(Guid extraordinaryEvaluationId);
        Task<object> GetStudentsDatatableClientSide(Guid extraordinaryEvaluationId);
        Task<bool> IsPendingFromQualify(Guid extraordinaryEvaluationId);

        //Task<ExtraordinaryEvaluationStudent> GetWithData(Guid id);
        Task<object> GetEnrollmentDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);
        Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationIdWithData(Guid extraordinaryEvaluationId);
        //Task<object> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string teacherId = null, Guid? courseId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentCurrentEvaluationsClientSideDatatable(Guid studentId, string searchValue);
    }
}
