using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IEmailManagementRepository : IRepository<EmailManagement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetEmailManagementDatatable(DataTablesStructs.SentParameters parameters, int? system);
        Task<bool> AnyBySystem(int system, Guid? ignoredId = null);
    }
}
