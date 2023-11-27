using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IMeetingUserRepository : IRepository<MeetingUser>
    {
        Task<DataTablesStructs.ReturnedData<MeetingUser>> GetMeetingUsersDatatable(DataTablesStructs.SentParameters sentParameters,Guid meetingId, string searchValue = null);
        Task<IEnumerable<MeetingUser>> GetMeetingUsersByMeetingId(Guid meetingId);
    }
}
