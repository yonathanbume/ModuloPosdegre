using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ICafeteriaServiceTermService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaServiceTermByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId,string searchValue = null);
        Task Insert(CafeteriaServiceTerm cafeteriaServiceTerm);
        Task Update(CafeteriaServiceTerm cafeteriaServiceTerm);
        Task Delete(Guid id);
        Task<CafeteriaServiceTerm> FirstOrDefaultById(Guid id);
        Task<CafeteriaServiceTerm> GetActiveCafeteriaServiceTerm();
        Task<object> GetCustom(Guid id);
    }
}
