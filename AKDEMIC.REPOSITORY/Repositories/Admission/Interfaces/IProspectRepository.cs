using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IProspectRepository : IRepository<Prospect>
    {
        Task<DataTablesStructs.ReturnedData<Prospect>> GetDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<bool> AnyInRange(int start, int end);
        Task<bool> Exists(int folderNum);
        Task<List<Prospect>> GetActiveProspects();
        Task<bool> CanDelete(Guid id);
        Task<Prospect> GetByNumber(int folderNum);
    }
}
