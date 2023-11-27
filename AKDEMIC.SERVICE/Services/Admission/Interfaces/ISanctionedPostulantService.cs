using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.SanctionedPostulant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface ISanctionedPostulantService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSanctionedPostulantDatatable(DataTablesStructs.SentParameters parameters, Guid? applicationTermId, string search);
        Task<bool> AnyByDni(string dni, Guid? applicationTermId, Guid? ignoredId = null);
        Task Delete(SanctionedPostulant entity);
        Task<SanctionedPostulant> Get(Guid id);
        Task<IEnumerable<SanctionedPostulant>> GetAll();
        Task Update(SanctionedPostulant entity);
        Task Insert(SanctionedPostulant entity);
        Task InsertRange(List<SanctionedPostulant> entities);
        Task<bool> SanctionedDNI(string dni, Guid applicationTermId);
        Task<List<SanctionedPostulantTemplate>> GetSanctionedPostulantData(Guid? applicationTermId, string search);
    }
}
