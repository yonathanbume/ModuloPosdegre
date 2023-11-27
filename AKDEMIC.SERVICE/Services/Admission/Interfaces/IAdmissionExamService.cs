using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamService
    {
        Task Insert(AdmissionExam admissionExam);
        Task Update(AdmissionExam admissionExam);
        Task DeleteExamById(Guid id);
        Task<AdmissionExam> Get(Guid id);
        Task<IEnumerable<AdmissionExam>> GetAll();
        Task<object> GetAmissionExam();
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
