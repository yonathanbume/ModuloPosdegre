using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IPrivateManagementPensionFundRepository:IRepository<PrivateManagementPensionFund>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPayrollPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conceptTypeId = null, string searchValue = null);
    }
}
