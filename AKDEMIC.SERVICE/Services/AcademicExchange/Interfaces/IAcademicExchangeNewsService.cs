using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IAcademicExchangeNewsService
    {
        Task Insert(AcademicExchangeNews entity);
        Task Delete(AcademicExchangeNews entity);
        Task<AcademicExchangeNews> Get(Guid id);
        Task<int> Count();
        Task Update(AcademicExchangeNews entity);
        Task<DataTablesStructs.ReturnedData<AcademicExchangeNews>> GetAcademicExchangeNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<AcademicExchangeNews>> GetAll();
        Task<IEnumerable<AcademicExchangeNews>> GetAllServerSide(int rowsPerPage, int page);
    }
}
