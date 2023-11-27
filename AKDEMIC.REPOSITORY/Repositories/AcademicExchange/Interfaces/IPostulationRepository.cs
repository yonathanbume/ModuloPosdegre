using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IPostulationRepository : IRepository<Postulation>
    {
        Task<bool> WasAnsweredByuser(Guid questionnaireId, string userId, string externalUser = null);
        Task<Guid> InsertAndReturnId(Postulation questionnaireByUser);
        Task<Postulation> GetQuestionnaireByPostulationId(Guid postulationId);
        Task<List<PostulationTemplate>> GetPostulationsReport(Guid? scholarshipId = null, byte? state = null);
        Task<DataTablesStructs.ReturnedData<Postulation>> GetPostulationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null);
        Task<object> GetPostulantsByscholarship();
        Task<DataTablesStructs.ReturnedData<Postulation>> GetAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? scholarshipId = null, byte? state = null, string search = null);
        Task<object> GetAdmittedByscholarship();
        Task<object> GetAdmittedByProgram();
        Task<object> GetPostulantsByProgram();
        Task<Postulation> GetByUserIdAndScholarshipId(Guid scholarshipId, string userId);
        Task<Postulation> GetByUserIdAndQuestionnaireId(string userId, Guid questionnaireId);
        Task<object> GetReportByCareerChart();
        Task<object> GetReportByTerm(Guid termId);
    }
}
