using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IEmailManagementService
    {
        Task<bool> AnyBySytem(int system, Guid? ignoredId = null);
        Task<EmailManagement> Get(Guid id);
        Task Insert(EmailManagement entity);
        Task Update(EmailManagement entity);
        Task Delete(EmailManagement entity);
        Task<DataTablesStructs.ReturnedData<object>> GetEmailManagementDatatable(DataTablesStructs.SentParameters parameters, int? system);
    }
}
