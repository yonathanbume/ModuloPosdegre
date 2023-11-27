using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.MeetingFile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IMeetingFileRepository : IRepository<MeetingFile>
    {
        Task<IEnumerable<MeetingFile>> GetMeetingFilesByMeetingId(Guid meetingId);
        Task<DataTablesStructs.ReturnedData<MeetingFileTemplate>> GetMeetingFilesDatatable(DataTablesStructs.SentParameters parameters, Guid id);
    }
}
