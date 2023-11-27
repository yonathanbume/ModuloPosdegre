using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.UserEvent;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class UserEventService: IUserEventService
    {
        private readonly IUserEventRepository _userEventRepo;
        public UserEventService(IUserEventRepository userEventRepo)
        {
            _userEventRepo = userEventRepo;
        }

        public async Task InsertUserEvent(UserEvent userEvent) =>
            await _userEventRepo.Insert(userEvent);

        public async Task UpdateUserEvent(UserEvent userEvent) =>
            await _userEventRepo.Update(userEvent);

        public async Task DeleteUserEvent(UserEvent userEvent) =>
            await _userEventRepo.Delete(userEvent);

        public async Task<UserEvent> GetUserEventById(Guid id) =>
            await _userEventRepo.Get(id);

        public async Task<ReportEventTemplate> GetUserEventByEventId(Guid eid)
        {
            return await _userEventRepo.GetUserEventByEventId(eid);
        }
        public async Task<IEnumerable<UserEvent>> GetUserEventsByEventId(Guid eid)
        {
            return await _userEventRepo.GetUserEventsByEventId(eid);
        }

        public async Task<List<StudentEnrolledTemplate>> GetRegisteredByEventId(Guid eid)
            => await _userEventRepo.GetRegisteredByEventId(eid);

        public async Task<object> GetAssistanceDetailByEventId(Guid eid)
            => await _userEventRepo.GetAssistanceDetailByEventId(eid);

        public async Task<List<UserEvent>> GetAllUserEvent()
            => await _userEventRepo.GetAllUserEvent();

        public async Task<object> GetEventUserInscription()
            => await _userEventRepo.GetEventUserInscription();

        public Task Save()
            => _userEventRepo.Save();

        public Task<UserEvent> GetByExternalUserAndEventId(Guid eventId, Guid externalUserId)
            => _userEventRepo.GetByExternalUserAndEventId(eventId, externalUserId);

        public Task<bool> IsUserSignedToEvent(string userId, Guid eventId)
            => _userEventRepo.IsUserSignedToEvent(userId, eventId);

        public Task<bool> ValidateRole(string userId, Guid eventId)
            => _userEventRepo.ValidateRole(userId, eventId);

        public Task<bool> ValidateCareer(string userId, Guid eventId)
            => _userEventRepo.ValidateCareer(userId, eventId);

        public Task<DataTablesStructs.ReturnedData<object>> GetUserEventDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string userId = null)
            => _userEventRepo.GetUserEventDatatable(sentParameters, searchValue, userId);

        public Task<UserEventPDFTemplate> GetCertificateTemplate(Guid id)
            => _userEventRepo.GetCertificateTemplate(id);
    }
}
