using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IUserAnnouncementRepository : IRepository<UserAnnouncement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<UserAnnouncement> GetActive(string userId);
        Task<bool> AnyInDates(DateTime startDate, DateTime endDate, Guid? id);
    }
}
