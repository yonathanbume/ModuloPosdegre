using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces
{
    public interface ISorterService
    {
        Task<bool> AnyByName(string name,Guid? ignoredId = null);
        Task Insert(Sorter sorter);
        Task Delete(Sorter sorter);
        Task<Sorter> Get(Guid id);
        Task Update(Sorter sorter);
        Task<IEnumerable<Select2Structs.Result>> GetSorterSelect2ClientSide();
        Task<DataTablesStructs.ReturnedData<Sorter>> GetSorterDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
    }
}
