using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.MeetingFile;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class MeetingFileService : IMeetingFileService
    {
        private readonly IMeetingFileRepository _meetingFileRepository;

        public MeetingFileService(IMeetingFileRepository meetingFileRepository)
        {
            _meetingFileRepository = meetingFileRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _meetingFileRepository.DeleteById(id);
        }

        public async Task DeleteRange(IEnumerable<MeetingFile> meetingFiles)
        {
            await _meetingFileRepository.DeleteRange(meetingFiles);
        }

        public async Task<MeetingFile> Get(Guid id)
        {
            return await _meetingFileRepository.Get(id);
        }

        public async Task<IEnumerable<MeetingFile>> GetMeetingFilesByMeetingId(Guid meetingId)
        {
            return await _meetingFileRepository.GetMeetingFilesByMeetingId(meetingId);
        }

        public async Task<DataTablesStructs.ReturnedData<MeetingFileTemplate>> GetMeetingFilesDatatable(DataTablesStructs.SentParameters parameters, Guid id)
        {
            return await _meetingFileRepository.GetMeetingFilesDatatable(parameters, id);
        }

        public async Task Insert(MeetingFile meetingFile)
        {
            await _meetingFileRepository.Insert(meetingFile);
        }

        public async Task Update(MeetingFile meetingFile)
        {
            await _meetingFileRepository.Update(meetingFile);
        }
    }
}
