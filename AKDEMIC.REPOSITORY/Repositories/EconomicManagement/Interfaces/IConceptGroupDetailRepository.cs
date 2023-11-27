using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptGroupDetailRepository : IRepository<ConceptGroupDetail>
    {
        Task<List<ConceptGroupDetail>> GetAllByGroupId(Guid conceptGroupId);
        Task<List<ConceptGroupDetail>> GetAllWithDataByGroupId(Guid conceptGroupId);
        Task<DataTablesStructs.ReturnedData<object>> GetGroupDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptGroupId);
    }
}
