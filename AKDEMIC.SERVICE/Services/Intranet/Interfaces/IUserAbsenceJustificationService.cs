using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IUserAbsenceJustificationService
    {
        Task<IEnumerable<UserAbsenceJustification>> GetAll(Guid? termId = null, string userId = null);
        Task<UserAbsenceJustification> Get(Guid id);
        Task Insert(UserAbsenceJustification userAbsenceJustification);
        Task Update(UserAbsenceJustification userAbsenceJustification);
        Task DeleteById(Guid id);
        Task Delete(UserAbsenceJustification userAbsenceJustification);
        Task<bool> Any(Guid workingDayId, int? status = null);
    }
}
