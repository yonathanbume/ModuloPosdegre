using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces
{
    public interface ISorterRepository : IRepository<Sorter>
    {
        Task<bool> AnyByName(string name,Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<Sorter>> GetSorterDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetSorterSelect2ClientSide();
    }
}
