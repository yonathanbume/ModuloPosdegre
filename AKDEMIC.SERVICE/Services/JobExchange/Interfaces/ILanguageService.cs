using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetLanguageDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(Language language);
        Task Update(Language language);
        Task Delete(Language language);
        Task<Language> Get(Guid id);
        Task<object> GetSelect2CLientSide();
    }
}
