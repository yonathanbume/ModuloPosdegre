using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SurveyUserService: ISurveyUserService
    {
        private readonly ISurveyUserRepository _surveyUserRepository;

        public SurveyUserService(ISurveyUserRepository surveyUserRepository)
        {
            _surveyUserRepository = surveyUserRepository;
        }

        public async Task AddRange(IEnumerable<SurveyUser> surveyUsers)
            => await _surveyUserRepository.AddRange(surveyUsers);

        public async Task<SurveyUser> Get(Guid id)
        {
            return await _surveyUserRepository.Get(id);
        }

        public async Task<IEnumerable<SurveyUser>> GetAllFirstLevelByUser(string userId)
        {
            return await _surveyUserRepository.GetAllFirstLevelByUser(userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBySurveyIdAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null)
        {
            return await _surveyUserRepository.GetBySurveyIdAnswersDatatable(sentParameters,surveyId,searchValue);
        }

        public async Task<SurveyUser> GetIncludeFirstLevel(Guid id)
        {
            return await _surveyUserRepository.GetIncludeFirstLevel(id);
        }

        public async Task<IEnumerable<SurveyUser>> GetInterestGroupUserSurveisByUserId(string UserId, Guid? academicProgramId = null)
        {
            return await _surveyUserRepository.GetInterestGroupUserSurveisByUserId(UserId,academicProgramId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveysByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId, DateTime? PublicationDate, DateTime? FinishDate, int type = 0, int system = 0)
        {
            return await _surveyUserRepository.GetSurveysByUserDatatable(sentParameters, userId, PublicationDate, FinishDate, type, system);
        }

        public async Task<IEnumerable<SurveyUser>> GetSurveyUsersBySurveyIdAsync(Guid surveyId)
        {
            return await _surveyUserRepository.GetSurveyUsersBySurveyId(surveyId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersInGeneralSurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null, bool? answered = null)
        {
            return await _surveyUserRepository.GetUsersInGeneralSurveyDatatable(sentParameters,surveyId,searchValue,answered);
        }

        public async  Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveysDataTable(DataTablesStructs.SentParameters parameters, Guid eid)
        {
            return await _surveyUserRepository.GetUsersBySurveysDataTable(parameters, eid);
        }

        public async Task<bool> IsSurveySendedToUsers(Guid id)
        {
            return await _surveyUserRepository.IsSurveySendedToUsers(id);
        }

        public async Task Update(SurveyUser surveyUser)
        {
            await _surveyUserRepository.Update(surveyUser);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyByCompanyJobExchangeDatatable(DataTablesStructs.SentParameters sentParameters, string userId,int surveyType = 0, string searchValue = null)
        {
            return await _surveyUserRepository.GetSurveyByCompanyJobExchangeDatatable(sentParameters,userId,surveyType,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null)
        {
            return await _surveyUserRepository.GetSurveyUserDatatable(sentParameters,surveyId,searchValue);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersBySurveyId(Guid surveyId)
        {
            return await _surveyUserRepository.GetUsersBySurveyId(surveyId);
        }

        public async Task<IEnumerable<SurveyUser>> GetLastPendingSurveyByUser(int take, string userId, int system)
            => await _surveyUserRepository.GetLastPendingSurveyByUser(take, userId, system);

        public Task<SurveyUser> GetFirstUserSurvey(bool isRequired, string userId)
            => _surveyUserRepository.GetFirstUserSurvey(isRequired,userId);

        public Task<SurveyUserTemplate> GetSurveyUserTemplate(Guid id)
            => _surveyUserRepository.GetSurveyUserTemplate(id);

        public Task<bool> AtLeastOneIsStudent(Guid surveyId)
            => _surveyUserRepository.AtLeastOneIsStudent(surveyId);
    }
}
