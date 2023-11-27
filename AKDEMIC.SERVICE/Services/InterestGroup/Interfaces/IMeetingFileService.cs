using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.MeetingFile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IMeetingFileService
    {
        Task<IEnumerable<MeetingFile>> GetMeetingFilesByMeetingId(Guid meetingId);
        Task DeleteRange(IEnumerable<MeetingFile> meetingFiles);
        Task Insert(MeetingFile meetingFile);
        Task Update(MeetingFile meetingFile);
        Task DeleteById(Guid id);
        Task<MeetingFile> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<MeetingFileTemplate>> GetMeetingFilesDatatable(DataTablesStructs.SentParameters parameters, Guid id);
    }
}
