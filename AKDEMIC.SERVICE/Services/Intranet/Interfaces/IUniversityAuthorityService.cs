using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IUniversityAuthorityService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthority(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistAuthorityType(int type, Guid? universityAuthorityId);
        Task Insert(UniversityAuthority newEntity);
        Task<UniversityAuthority> Get(Guid id);
        Task Update(UniversityAuthority universityAuthority);
        Task<List<UniversityAuthority>> GetUniversityAuthoritiesList();
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthorityHistory(Guid id);
    }
}
