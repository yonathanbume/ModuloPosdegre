using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupSurveyTemplate;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupSurveyService : IInterestGroupSurveyService
    {
        private readonly IInterestGroupSurveyRepository _interestGroupSurveyRepository;

        public InterestGroupSurveyService(IInterestGroupSurveyRepository interestGroupSurveyRepository)
        {
            _interestGroupSurveyRepository = interestGroupSurveyRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<SurveyTemplate>> GetInterestGroupSurveys(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId, string search, int year, int month, string userDegreeProgramId = null, Guid? interestGroupId = null, byte? status = null)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveys(sentParameters,academicProgramId, search, year, month,userDegreeProgramId,interestGroupId,status);
        }

        public async Task<IEnumerable<InterestGroupSurvey>> GetInterestGroupSurveysByInterestGroupId(Guid interestGroupId)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveysByInterestGroupId(interestGroupId);
        }

        public async Task<Select2Structs.ResponseParameters> GetInterestGroupSurveysByInterestGroupIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? interestGroupId, string searchValue = null)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveysByInterestGroupIdSelect2(requestParameters, interestGroupId, searchValue);
        }

        public async Task Insert(InterestGroupSurvey interestGroupSurvey)
        {
            await _interestGroupSurveyRepository.Insert(interestGroupSurvey);
        }

        public async Task DeleteBySurveyId(Guid Id)
        {
            await _interestGroupSurveyRepository.DeleteBySurveyId(Id);
        }

        public async Task<IEnumerable<SurveyUser>> GetInterestGroupSurveyUsersByUserIdAndInterestGroupId(string userId, Guid interestGroupId)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveyUsersByUserIdAndInterestGroupId(userId, interestGroupId);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetInterestGroupSurveysByInterestGroupIdSelect2ClientSide(Guid interestGroupId)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveysByInterestGroupIdSelect2ClientSide(interestGroupId);
        }

        public async Task<InterestGroupSurvey> GetInterestGroupSurveyBySurveyUser(Guid surveyUserId)
        {
            return await _interestGroupSurveyRepository.GetInterestGroupSurveyBySurveyUser(surveyUserId);
        }

        public async Task<int> GetCountSlopesSurveysByUserId(string userId)
        {
            return await _interestGroupSurveyRepository.GetCountSlopesSurveysByUserId(userId);
        }
    }
}
