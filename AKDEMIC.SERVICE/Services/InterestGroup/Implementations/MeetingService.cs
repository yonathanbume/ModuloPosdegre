using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.Meeting;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingService(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task Delete(Meeting meeting)
            => await _meetingRepository.Delete(meeting);

        public async Task DeleteById(Guid id)
           => await _meetingRepository.DeleteById(id);

        public async Task<bool> ExistCodeByInterestGroupId(Guid interestGroupId, string code, Guid? currentMeetingId = null)
            => await _meetingRepository.ExistCodeByInterestGroupId(interestGroupId, code, currentMeetingId);

        public async Task<Meeting> Get(Guid id)
            => await _meetingRepository.Get(id);

        public async Task<int> GetCountByInterestGroupId(Guid interestGroupId)
            => await _meetingRepository.GetCountByInterestGroupId(interestGroupId);

        public async Task<DataTablesStructs.ReturnedData<MeetingReportTemplate>> GetMeetingReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null)
            => await _meetingRepository.GetMeetingReportDatatable(sentParameters, academicProgramId, searchValue);

        public async Task<IEnumerable<Meeting>> GetMeetingsByInterestGroupId(Guid interestGroupId)
            => await _meetingRepository.GetMeetingsByInterestGroupId(interestGroupId);

        public async Task<DataTablesStructs.ReturnedData<MeetingTemplate>> GetMeetingsDatatable(DataTablesStructs.SentParameters parameters, Guid interestGroupId, string date, string matter, string number)
        {
            return await _meetingRepository.GetMeetingsDatatable(parameters, interestGroupId, date, matter, number);
        }

        public async Task Insert(Meeting meeting)
            => await _meetingRepository.Insert(meeting);

        public async Task Update(Meeting meeting)
            => await _meetingRepository.Update(meeting);

        public async Task<IEnumerable<MeetingReportTemplate>> GetMeetingReportData(Guid? interestGroupId)
        {
            return await _meetingRepository.GetMeetingReportData(interestGroupId);
        }

        public async Task<int> Count()
        {
            return await _meetingRepository.Count();
        }

        public async Task<DataTablesStructs.ReturnedData<Meeting>> GetMeetingDetailsByUserIdDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _meetingRepository.GetMeetingDetailsByUserIdDatatable(sentParameters, userId);
    }
}
