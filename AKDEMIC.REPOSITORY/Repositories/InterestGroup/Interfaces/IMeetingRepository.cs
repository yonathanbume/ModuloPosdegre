using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.Meeting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        Task<IEnumerable<Meeting>> GetMeetingsByInterestGroupId(Guid interestGroupId);
        Task<bool> ExistCodeByInterestGroupId(Guid interestGroupId, string code, Guid? currentMeetingId = null);
        Task<DataTablesStructs.ReturnedData<MeetingTemplate>> GetMeetingsDatatable(DataTablesStructs.SentParameters parameters, Guid interestGroupId, string date, string matter, string number);
        Task<int> GetCountByInterestGroupId(Guid interestGroupId);
        Task<DataTablesStructs.ReturnedData<MeetingReportTemplate>> GetMeetingReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null);
        Task<IEnumerable<MeetingReportTemplate>> GetMeetingReportData(Guid? interestGroupId);
        Task<DataTablesStructs.ReturnedData<Meeting>> GetMeetingDetailsByUserIdDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
