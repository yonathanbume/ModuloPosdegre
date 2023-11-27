using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.Meeting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IMeetingService
    {
        Task<IEnumerable<Meeting>> GetMeetingsByInterestGroupId(Guid interestGroupId);
        Task<bool> ExistCodeByInterestGroupId(Guid interestGroupId, string code, Guid? currentMeetingId = null);
        Task Delete(Meeting meeting);
        Task Insert(Meeting meeting);
        Task Update(Meeting meeting);
        Task<Meeting> Get(Guid id);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<MeetingTemplate>> GetMeetingsDatatable(DataTablesStructs.SentParameters parameters, Guid interestGroupId, string date, string matter, string number);
        Task<int> GetCountByInterestGroupId(Guid interestGroupId);
        Task<DataTablesStructs.ReturnedData<MeetingReportTemplate>> GetMeetingReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null);
        Task<IEnumerable<MeetingReportTemplate>> GetMeetingReportData(Guid? interestGroupId);
        Task<int> Count();
        Task<DataTablesStructs.ReturnedData<Meeting>> GetMeetingDetailsByUserIdDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
