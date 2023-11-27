using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IInterestGroupFileService
    {
        Task<DataTablesStructs.ReturnedData<InterestGroupFile>> GetInterestGroupFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid interestGroupId, string searchValue = null);
        Task<InterestGroupFile> Get(Guid id);
        Task Insert(InterestGroupFile entity);
        Task Update(InterestGroupFile entity);
        Task Delete(InterestGroupFile entity);
    }
}
