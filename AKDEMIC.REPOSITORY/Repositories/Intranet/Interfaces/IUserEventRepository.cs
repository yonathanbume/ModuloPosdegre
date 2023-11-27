using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.UserEvent;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IUserEventRepository : IRepository<UserEvent>
    {
        Task<ReportEventTemplate> GetUserEventByEventId(Guid eid);
        Task<IEnumerable<UserEvent>> GetUserEventsByEventId(Guid eid);
        Task<List<StudentEnrolledTemplate>> GetRegisteredByEventId(Guid eid);
        Task<UserEvent> GetByExternalUserAndEventId(Guid eventId, Guid externalUserId);
        Task<object> GetAssistanceDetailByEventId(Guid eid);
        Task Save();
        Task<List<UserEvent>> GetAllUserEvent();
        Task<UserEventPDFTemplate> GetCertificateTemplate(Guid id);
        Task<bool> IsUserSignedToEvent(string userId, Guid eventId);
        Task<object> GetEventUserInscription();
        Task<bool> ValidateRole(string userId, Guid eventId);
        Task<bool> ValidateCareer(string userId, Guid eventId);
        Task<DataTablesStructs.ReturnedData<object>> GetUserEventDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string userId = null);
    }
}
