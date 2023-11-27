using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IConceptGroupDetailService
    {
        Task<List<ConceptGroupDetail>> GetAllByGroupId(Guid conceptGroupId);
        Task<List<ConceptGroupDetail>> GetAllWithDataByGroupId(Guid conceptGroupId);
        Task<ConceptGroupDetail> Get(Guid grouptId, Guid conceptId);
        Task Insert(ConceptGroupDetail detail);
        Task Update(ConceptGroupDetail detail);
        Task Delete(ConceptGroupDetail detail);
        Task DeleteRange(IEnumerable<ConceptGroupDetail> details);
        Task<DataTablesStructs.ReturnedData<object>> GetGroupDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid groupId);

    }
}
