using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IUITService
    {
        Task<bool> AnyUITByYear(int year);
        Task<UIT> Get(Guid id);
        Task<UIT> GetCurrentUIT();
        Task<IEnumerable<UIT>> GetAll();
        Task<DataTablesStructs.ReturnedData<UIT>> GetUITsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Delete(UIT uit);
        Task Insert(UIT uit);
        Task Update(UIT uit);
        Task<UIT> LastOrDefaultAsync();
    }
}
