using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IInterestGroupFileRepository : IRepository<InterestGroupFile>
    {
        Task<DataTablesStructs.ReturnedData<InterestGroupFile>> GetInterestGroupFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid interestGroupId, string searchValue = null);
    }
}
