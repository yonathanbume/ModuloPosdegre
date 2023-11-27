using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISurveyUserService
    {
        Task Update(SurveyUser surveyUser);
        Task<SurveyUser> GetFirstUserSurvey(bool isRequired, string userId);
        Task<IEnumerable<SurveyUser>> GetLastPendingSurveyByUser(int take,string userId,int survey);
        Task AddRange(IEnumerable<SurveyUser> surveyUsers);
        Task<SurveyUserTemplate> GetSurveyUserTemplate(Guid id);
        Task<SurveyUser> Get(Guid id);
        Task<IEnumerable<SurveyUser>> GetAllFirstLevelByUser(string userId);
        Task<IEnumerable<SurveyUser>> GetSurveyUsersBySurveyIdAsync(Guid surveyId);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveysByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId, DateTime? PublicationDate, DateTime? FinishDate, int type = 0, int system = 0);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveyByCompanyJobExchangeDatatable(DataTablesStructs.SentParameters sentParameters, string userId,int surveyType = 0, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetBySurveyIdAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersInGeneralSurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null, bool? answered = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null);
        Task<SurveyUser> GetIncludeFirstLevel(Guid id);
        Task<bool> AtLeastOneIsStudent(Guid surveyId);
        Task<bool> IsSurveySendedToUsers(Guid id);
        Task<IEnumerable<SurveyUser>> GetInterestGroupUserSurveisByUserId(string UserId, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveysDataTable(DataTablesStructs.SentParameters parameters, Guid eid);
        Task<IEnumerable<ApplicationUser>> GetUsersBySurveyId(Guid surveyId);
    }
}
