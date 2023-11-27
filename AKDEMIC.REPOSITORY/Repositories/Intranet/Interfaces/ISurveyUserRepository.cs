using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISurveyUserRepository :IRepository<SurveyUser>
    {
        Task<SurveyUserTemplate> GetSurveyUserTemplate(Guid id);
        Task<SurveyUser> GetFirstUserSurvey(bool isRequired, string userId);
        Task<IEnumerable<SurveyUser>> GetLastPendingSurveyByUser(int take, string userId,int survey);
        Task<SurveyUser> GetIncludeFirstLevel(Guid id);
        Task<IEnumerable<SurveyUser>> GetAllFirstLevelByUser(string userId);
        Task<bool> IsSurveySendedToUsers(Guid id);
        Task<bool> AtLeastOneIsStudent(Guid surveyId);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveysByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId, DateTime? PublicationDate, DateTime? FinishDate, int type = 0, int system = 0);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveyByCompanyJobExchangeDatatable(DataTablesStructs.SentParameters sentParameters, string userId, int surveyType = 0 , string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetBySurveyIdAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersInGeneralSurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null, bool? answered = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null);
        Task<IEnumerable<SurveyUser>> GetSurveyUsersBySurveyId(Guid surveyId);
        Task<IEnumerable<SurveyUser>> GetInterestGroupUserSurveisByUserId(string userId, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveysDataTable(DataTablesStructs.SentParameters parameters, Guid eid);
        Task<IEnumerable<ApplicationUser>> GetUsersBySurveyId(Guid surveyId);
    }
}
