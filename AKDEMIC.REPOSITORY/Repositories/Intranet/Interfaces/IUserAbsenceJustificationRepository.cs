using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IUserAbsenceJustificationRepository : IRepository<UserAbsenceJustification>
    {
        Task<IEnumerable<UserAbsenceJustification>> GetAll(Guid? termId = null, string userId = null);
        Task<bool> Any(Guid workingDayId, int? status = null);
    }
}
