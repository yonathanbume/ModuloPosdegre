using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupSurveyTemplate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IInterestGroupSurveyRepository : IRepository<InterestGroupSurvey>
    {
        Task<IEnumerable<InterestGroupSurvey>> GetInterestGroupSurveysByInterestGroupId(Guid interestGroupId);
        Task<Select2Structs.ResponseParameters> GetInterestGroupSurveysByInterestGroupIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? interestGroupId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<SurveyTemplate>> GetInterestGroupSurveys(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId, string search, int year, int month, string userDegreeProgramId = null, Guid? interestGroupId = null, byte? status = null);
        Task<IEnumerable<SurveyUser>> GetInterestGroupSurveyUsersByUserIdAndInterestGroupId(string userId, Guid interestGroupId);
        Task<IEnumerable<Select2Structs.Result>> GetInterestGroupSurveysByInterestGroupIdSelect2ClientSide(Guid interestGroupId);
        Task DeleteBySurveyId(Guid surveyId);
        Task<InterestGroupSurvey> GetInterestGroupSurveyBySurveyUser(Guid id);
        Task<int> GetCountSlopesSurveysByUserId(string userId);
    }
}
