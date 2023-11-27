using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces
{
    public interface IInstitutionalInformationRepository : IRepository<InstitutionalInformation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalInformationDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, Guid? dependencyId = null, DateTime? publishDate = null, string search = null);
    }
}
