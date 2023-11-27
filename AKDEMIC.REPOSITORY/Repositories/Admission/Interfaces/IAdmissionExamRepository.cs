using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamRepository : IRepository<AdmissionExam>
    {
        Task<object> GetAmissionExam();
        Task DeleteExamById(Guid id);
        Task<AdmissionExam> GetWithData(Guid id);
        Task<List<AdmissionExam>> GetClassroomManagement(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<object> GetAdmissionExams();
        Task<List<AdmissionExam>> GetByApplicationTermId(Guid applicationTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetAdmissionExamDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantAssistanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid examId, byte status = 1);
        Task<List<AdmissionExam>> GetActiveApplicationTermsExams();
    }
}
