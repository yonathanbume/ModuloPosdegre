using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.UserEvent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IUserEventService
    {
        Task InsertUserEvent(UserEvent userEvent);
        Task UpdateUserEvent(UserEvent userEvent);
        Task Save();
        Task DeleteUserEvent(UserEvent userEvent);
        Task<UserEventPDFTemplate> GetCertificateTemplate(Guid id);
        Task<UserEvent> GetUserEventById(Guid id);
        Task<UserEvent> GetByExternalUserAndEventId(Guid eventId, Guid externalUserId);
        Task<ReportEventTemplate> GetUserEventByEventId(Guid eid);
        Task<IEnumerable<UserEvent>> GetUserEventsByEventId(Guid eid);
        Task<List<StudentEnrolledTemplate>> GetRegisteredByEventId(Guid eid);
        Task<object> GetAssistanceDetailByEventId(Guid eid);
        Task<List<UserEvent>> GetAllUserEvent();
        Task<bool> IsUserSignedToEvent(string userId, Guid eventId);
        Task<object> GetEventUserInscription();
        Task<bool> ValidateRole(string userId, Guid eventId);
        Task<bool> ValidateCareer(string userId, Guid eventId);
        Task<DataTablesStructs.ReturnedData<object>> GetUserEventDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string userId = null);
    }
}
