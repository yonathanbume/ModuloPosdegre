using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VirtualDirectory.Interfaces
{
    public interface IDirectoryDependencyService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPeopleInChargeDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependecyId, string search);
        Task<DirectoryDependency> Get(Guid id);
        Task Update(DirectoryDependency entity);
        Task Insert(DirectoryDependency entity);
        Task Delete(DirectoryDependency entity);
        Task<bool> HasPersonInCharge(Guid dependencyId, byte charge, Guid? id = null);
        Task<object> GetPeopleInChargeToDirectory(PaginationParameter paginationParameters, byte filterType, string searchValue);
    }
}
