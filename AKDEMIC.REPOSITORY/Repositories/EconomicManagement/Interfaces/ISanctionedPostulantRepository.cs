using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.SanctionedPostulant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ISanctionedPostulantRepository : IRepository<SanctionedPostulant>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSanctionedPostulantDatatable(DataTablesStructs.SentParameters parameters, Guid? applicationTermId, string search);
        Task<bool> AnyByDni(string dni, Guid? applicationTermId, Guid? ignoredId = null);
        Task<bool> SanctionedDNI(string dni, Guid applicationTermId);
        Task<List<SanctionedPostulantTemplate>> GetSanctionedPostulantData(Guid? applicationTermId, string search);
    }
}
