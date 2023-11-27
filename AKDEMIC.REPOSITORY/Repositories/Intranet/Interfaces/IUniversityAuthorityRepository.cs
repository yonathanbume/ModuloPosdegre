using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IUniversityAuthorityRepository : IRepository<UniversityAuthority>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthority(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistAuthorityType(int type, Guid? universityAuthorityId);
        Task<List<UniversityAuthority>> GetUniversityAuthoritiesList();
        Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthorityHistory(Guid id);
    }
}
