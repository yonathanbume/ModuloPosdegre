using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroup;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupService : IInterestGroupService
    {
        private readonly IInterestGroupRepository _interestGroupRepository;

        public InterestGroupService(IInterestGroupRepository interestGroupRepository)
        {
            _interestGroupRepository = interestGroupRepository;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupsByForumsSelect2ClientSide(Guid? forumId)
            => await _interestGroupRepository.GetActiveInterestGroupsByForumsSelect2ClientSide(forumId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInterestGroupsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, Guid? academicProgramId = null, string date = null,string userId=null, string searchValue = null)
            => await _interestGroupRepository.GetInterestGroupsDatatable(sentParameters, name, academicProgramId, date, userId, searchValue);

        public async Task Insert(ENTITIES.Models.InterestGroup.InterestGroup entity)
            => await _interestGroupRepository.Insert(entity);

        public async Task Update(ENTITIES.Models.InterestGroup.InterestGroup entity)
            => await _interestGroupRepository.Update(entity);

        public async Task Delete(ENTITIES.Models.InterestGroup.InterestGroup entity)
            => await _interestGroupRepository.Delete(entity);

        public async Task<ENTITIES.Models.InterestGroup.InterestGroup> Get(Guid id)
            => await _interestGroupRepository.Get(id);

        public async Task<bool> ExistActiveGroupByAcademicProgramId(Guid academicProgramId, DateTime startdate, DateTime endDate)
            => await _interestGroupRepository.ExistActiveGroupByAcademicProgramId(academicProgramId, startdate, endDate);

        public async Task<int> GetCountOfInterestGroupByUserAdminId(string userAdminId)
            => await _interestGroupRepository.GetCountOfInterestGroupByUserAdminId(userAdminId);

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, Guid? id, string p)
        {
            return await _interestGroupRepository.GetActiveInterestGroupsByForumsSelect2(requestParameters, id, p);
        }

        public async Task<ApplicationUser> GetUserAdminByInterestGroupId(Guid interestGroupId)
            => await _interestGroupRepository.GetUserAdminByInterestGroupId(interestGroupId);

        public async Task<DataTablesStructs.ReturnedData<ActivityReportTemplate>> GetActivityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null)
            => await _interestGroupRepository.GetActivityReportDatatable(sentParameters, academicProgramId, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserAdminIdSelect2(Select2Structs.RequestParameters requestParameters, string userAdminId, string searchValue = null)
            => await _interestGroupRepository.GetActiveInterestGroupByUserAdminIdSelect2(requestParameters, userAdminId, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, string UserId, Guid? id, string p) 
            => await _interestGroupRepository.GetActiveInterestGroupsByForumsSelect2(requestParameters, UserId, id, p);

        public async Task<IEnumerable<ActivityReportTemplate>> GetActivityReportData(Guid? interestGroupId)
        {
            return await _interestGroupRepository.GetActivityReportData(interestGroupId);
        }

        public async Task<ENTITIES.Models.InterestGroup.InterestGroup> GetAcademicProgramByInterestGroupId(Guid interestGroupId)
        {
            return await _interestGroupRepository.GetAcademicProgramByInterestGroupId(interestGroupId);
        }

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
            => await _interestGroupRepository.GetActiveInterestGroupByUserIdSelect2(requestParameters, userId, searchValue);

        public async Task<IEnumerable<ENTITIES.Models.InterestGroup.InterestGroup>> GetActiveInterestGroupsByUserId(string userId)
            => await _interestGroupRepository.GetActiveInterestGroupsByUserId(userId);

        public async Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupByUserIdSelect2ClientSide(string userId)
            => await _interestGroupRepository.GetActiveInterestGroupByUserIdSelect2ClientSide(userId);

        public async Task<object> GetSurveyReportChart(Guid interestGroupId)
            => await _interestGroupRepository.GetSurveyReportChart(interestGroupId);

        public async Task<object> GetInterestGroupActiveSelect2()
            => await _interestGroupRepository.GetInterestGroupActiveSelect2();
    }
}
