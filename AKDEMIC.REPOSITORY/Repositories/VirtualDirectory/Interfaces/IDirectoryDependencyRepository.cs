using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces
{
    public interface IDirectoryDependencyRepository : IRepository<DirectoryDependency>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPeopleInChargeDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependecyId, string search);
        Task<bool> HasPersonInCharge(Guid dependencyId, byte charge, Guid? id = null);
        Task<object> GetPeopleInChargeToDirectory(PaginationParameter paginationParameters, byte filterType, string searchValue);
    }
}
