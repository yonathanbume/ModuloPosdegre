using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryTermService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null);
        Task<PreuniversitaryTerm> GetActivePreUniversitaryApplicationTerm();
        Task<PreuniversitaryTerm> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> AnyByDateTimeConflict(DateTime parsedStartDate, DateTime parsedEndDate, Guid? ignoredId = null);
        Task Insert(PreuniversitaryTerm entity);
        Task Update(PreuniversitaryTerm entity);
        Task Delete(PreuniversitaryTerm entity);
        Task<object> GetPreunviersitaryTermSelect2ClientSide();
        Task<object> GetCurrentPreuniversitaryTerm();
    }
}
