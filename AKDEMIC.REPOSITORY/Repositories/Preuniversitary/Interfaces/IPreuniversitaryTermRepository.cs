using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryTermRepository : IRepository<PreuniversitaryTerm>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null);
        Task<PreuniversitaryTerm> GetActivePreUniversitaryApplicationTerm();
        Task<DataTablesStructs.ReturnedData<object>> GetTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> AnyByDateTimeConflict(DateTime parsedStartDate, DateTime parsedEndDate, Guid? ignoredId = null);
        Task<object> GetPreunviersitaryTermSelect2ClientSide();
        Task<object> GetCurrentPreuniversitaryTerm();
    }
}
