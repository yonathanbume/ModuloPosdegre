using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IMeetingUserService
    {
        Task<DataTablesStructs.ReturnedData<MeetingUser>> GetMeetingUsersDatatable(DataTablesStructs.SentParameters sentParameters,Guid meetingId, string searchValue = null);
        Task<IEnumerable<MeetingUser>> GetMeetingUsersByMeetingId(Guid meetingId);
        Task DeleteRange(IEnumerable<MeetingUser> entites);
    }
}
