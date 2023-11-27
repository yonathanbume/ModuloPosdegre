using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IProspectService
    {
        Task<DataTablesStructs.ReturnedData<Prospect>> GetDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<bool> AnyInRange(int start, int end);
        Task Insert(Prospect prospect);
        Task<Prospect> Get(Guid id);
        Task Delete(Prospect prospect);
        Task Update(Prospect prospect);
        Task<bool> Exists(int folderNum);
        Task<List<Prospect>> GetActiveProspects();
        Task<bool> CanDelete(Guid id);
        Task<Prospect> GetByNumber(int folderNum);
    }
}
