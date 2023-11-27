using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IIncubationCallService
    {
        Task<IncubationCall> IncubationCallAdmin();
        Task<IncubationCall> Get(Guid id);
        Task<IncubationCall> GetByUser(string userId);
        Task Update(IncubationCall incubationCall);
        Task Delete(IncubationCall incubationCall);
        Task Insert(IncubationCall incubationCall);        
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdmin2Datatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAceptedDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null);
        Task InsertRange(IEnumerable<IncubationCall> incubationCalls);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdminDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallNotAdminDatatable(DataTablesStructs.SentParameters sentParameters,string rolId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallEnterpriseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
