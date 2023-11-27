using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ICafeteriaServiceTermRepository : IRepository<CafeteriaServiceTerm>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId,string searchValue = null);
        Task<CafeteriaServiceTerm> FirstOrDefaultById(Guid id);
        Task<object> GetCustom(Guid id);
        Task<CafeteriaServiceTerm> GetActiveCafeteriaServiceTerm();
    }
}
