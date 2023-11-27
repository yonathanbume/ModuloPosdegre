using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IPrivateManagementPensionFundService
    {
        Task<PrivateManagementPensionFund> Get(Guid id);
        Task<IEnumerable<PrivateManagementPensionFund>> GetAll();
        Task<bool> AnyByName(string name, Guid? id = null);
        Task Insert(PrivateManagementPensionFund privateManagementPensionFund);
        Task Update(PrivateManagementPensionFund privateManagementPensionFund);
        Task Delete(PrivateManagementPensionFund privateManagementPensionFund);

        Task<DataTablesStructs.ReturnedData<object>> GetPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<DataTablesStructs.ReturnedData<object>> GetPayrollPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conceptTypeId = null,string searchValue = null);
    }
}
