using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IIncubationCallRepository : IRepository<IncubationCall>
    {
        Task<IncubationCall> IncubationCallAdmin();
        Task<IncubationCall> GetByUser(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdminDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallNotAdminDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallEnterpriseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);        
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdmin2Datatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAceptedDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null);
    }
}
