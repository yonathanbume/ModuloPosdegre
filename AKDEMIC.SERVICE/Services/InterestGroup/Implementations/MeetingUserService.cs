using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class MeetingUserService : IMeetingUserService
    {
        private readonly IMeetingUserRepository _meetingUserRepository;

        public MeetingUserService(IMeetingUserRepository meetingUserRepository)
        {
            _meetingUserRepository = meetingUserRepository;
        }

        public async Task DeleteRange(IEnumerable<MeetingUser> entites)
            => await _meetingUserRepository.DeleteRange(entites);

        public async Task<IEnumerable<MeetingUser>> GetMeetingUsersByMeetingId(Guid meetingId)
            => await _meetingUserRepository.GetMeetingUsersByMeetingId(meetingId);

        public async Task<DataTablesStructs.ReturnedData<MeetingUser>> GetMeetingUsersDatatable(DataTablesStructs.SentParameters sentParameters, Guid meetingId,string searchValue = null)
            => await _meetingUserRepository.GetMeetingUsersDatatable(sentParameters,meetingId, searchValue);

    }
}
