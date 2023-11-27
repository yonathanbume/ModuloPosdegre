using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ISupportOfficeUserService
    {
        Task<DataTablesStructs.ReturnedData<SupportOfficeUser>> GetSupportOfficeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null);
        Task<SupportOfficeUser> Get(string supportOfficeUserId);
        Task<IEnumerable<SupportOfficeUser>> GetAll();
        Task<SupportOfficeUser> Add(SupportOfficeUser supportOfficeUser);
        Task Insert(SupportOfficeUser supportOfficeUser);
        Task Update(SupportOfficeUser supportOfficeUser);
        Task Delete(SupportOfficeUser supportOfficeUser);
        Task DeleteById(string id);
        Task<SupportOfficeUser> GetByUser(string userId);
        Task<SupportOffice> GetSupportOfficeByUser(string userId);
    }
}
