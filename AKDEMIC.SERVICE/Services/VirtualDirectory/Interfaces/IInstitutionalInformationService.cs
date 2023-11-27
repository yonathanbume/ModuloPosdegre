using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VirtualDirectory.Interfaces
{
    public interface IInstitutionalInformationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalInformationDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, Guid? dependencyId = null, DateTime? publishDate = null, string search = null);
        Task<InstitutionalInformation> Get(Guid id);
        Task Update(InstitutionalInformation entity);
        Task Delete(InstitutionalInformation entity);
        Task Insert(InstitutionalInformation entity);
    }
}
