using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ISupportOfficeUserRepository : IRepository<SupportOfficeUser>
    {
        Task<DataTablesStructs.ReturnedData<SupportOfficeUser>> GetSupportOfficeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null);
        Task<SupportOfficeUser> GetByUser(string userId);
        Task<SupportOffice> GetSupportOfficeByUser(string userId);

    }
}
