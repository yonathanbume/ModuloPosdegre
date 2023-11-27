using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IInterestGroupRepository : IRepository<ENTITIES.Models.InterestGroup.InterestGroup>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInterestGroupsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, Guid? academicProgramId = null,  string date = null, string userId = null, string searchValue = null);
        Task<bool> ExistActiveGroupByAcademicProgramId(Guid academicProgramId, DateTime startdate, DateTime endDate);
        Task<int> GetCountOfInterestGroupByUserAdminId(string userAdminId);
        Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, Guid? id, string p);
        Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, string userId, Guid? id, string p);
        Task<ApplicationUser> GetUserAdminByInterestGroupId(Guid interestGroupId);
        Task<DataTablesStructs.ReturnedData<ActivityReportTemplate>> GetActivityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserAdminIdSelect2(Select2Structs.RequestParameters requestParameters, string userAdminId, string searchValue = null);
        Task<IEnumerable<ActivityReportTemplate>> GetActivityReportData(Guid? interestGroupId);
        Task<ENTITIES.Models.InterestGroup.InterestGroup> GetAcademicProgramByInterestGroupId(Guid interestGroupId);
        Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null);
        Task<IEnumerable<ENTITIES.Models.InterestGroup.InterestGroup>> GetActiveInterestGroupsByUserId(string userId);
        Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupsByForumsSelect2ClientSide(Guid? forumId);
        Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupByUserIdSelect2ClientSide(string userId);
        Task<object> GetSurveyReportChart(Guid interestGroupId);
        Task<object> GetInterestGroupActiveSelect2();
    }
}
