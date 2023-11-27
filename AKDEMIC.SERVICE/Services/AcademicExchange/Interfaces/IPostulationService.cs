using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IPostulationService
    {
        Task<DataTablesStructs.ReturnedData<Postulation>> GetPostulationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null);
        Task<bool> WasAnsweredByuser(Guid questionnaireId, string userId, string externalUser = null);
        Task<Guid> InsertAndReturnId(Postulation questionnaireByUser);
        Task<object> GetPostulantsByscholarship();
        Task<Postulation> GetQuestionnaireByPostulationId(Guid postulationId);
        Task<Postulation> Get(Guid id);
        Task<Postulation> GetByUserIdAndScholarshipId(Guid scholarshipId, string userId);

        Task<List<PostulationTemplate>> GetPostulationsReport(Guid? scholarshipId = null, byte? state = null);
        Task Update(Postulation entity);
        //Task<object> GetQuestionnaireByUserDetails(Guid questionnaireByUserId);
        Task<DataTablesStructs.ReturnedData<Postulation>> GetAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null);
        Task<object> GetAdmittedByscholarship();
        Task<object> GetAdmittedByProgram();
        Task<object> GetPostulantsByProgram();
        Task Add(Postulation postulation);
        Task Insert(Postulation postulation);
        Task<Postulation> GetByUserIdAndQuestionnaireId(string userId, Guid questionnaireId);
        Task<object> GetReportByCareerChart();
        Task<object> GetReportByTerm(Guid termId);
    }
}
