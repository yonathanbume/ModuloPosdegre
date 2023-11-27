using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface ILicenseResolutionTypeRepository:IRepository<LicenseResolutionType>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllLicenseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
