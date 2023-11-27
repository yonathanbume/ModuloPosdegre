using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IAcademicExchangeNewsRepository : IRepository<AcademicExchangeNews>
    {
        Task<DataTablesStructs.ReturnedData<AcademicExchangeNews>> GetAcademicExchangeNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<AcademicExchangeNews>> GetAllServerSide(int rowsPerPage, int page);
    }
}
