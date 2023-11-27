using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface ILicenseResolutionTypeService
    {
        Task<LicenseResolutionType> Get(Guid id);
        Task<IEnumerable<LicenseResolutionType>> GetAll();
        Task Insert(LicenseResolutionType licenseResolutionType);
        Task<bool> AnyByName(string name , Guid? id = null);
        Task Update(LicenseResolutionType licenseResolutionType);
        Task Delete(LicenseResolutionType licenseResolutionType);

        Task<DataTablesStructs.ReturnedData<object>> GetAllLicenseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
