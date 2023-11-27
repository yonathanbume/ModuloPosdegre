using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationStudentService
    {
        Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationId(Guid extraordinaryEvaluationId);
        Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationIdWithData(Guid extraordinaryEvaluationId);
        Task Insert(ExtraordinaryEvaluationStudent entity);
        Task<ExtraordinaryEvaluationReportTemplate> GetEvaluationReportInformation(Guid extraordinaryEvaluationId);
        Task<ExtraordinaryEvaluationStudent> Get(Guid Id);
        Task Delete(ExtraordinaryEvaluationStudent entity);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid extraordinaryEvaluationId, string searchValue);
        Task<bool> IsPendingFromQualify(Guid extraordinaryEvaluationId);
        Task<object> GetStudentsDatatableClientSide(Guid extraordinaryEvaluationId);

        //Task Insert(ExtraordinaryEvaluationStudent extraordinaryEvaluationStudent);
        Task Update(ExtraordinaryEvaluationStudent extraordinaryEvaluationStudent);
        //Task DeleteById(Guid id);
        //Task<ExtraordinaryEvaluationStudent> Get(Guid id);
        Task<object> GetEnrollmentDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);
        //Task<ExtraordinaryEvaluationStudent> GetWithData(Guid id);
        //Task<object> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string teacherId = null, Guid? courseId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentCurrentEvaluationsClientSideDatatable(Guid studentId, string searchValue);

    }
}
