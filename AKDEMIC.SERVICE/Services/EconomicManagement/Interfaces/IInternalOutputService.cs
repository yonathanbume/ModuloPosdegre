using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IInternalOutputService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInternalOuputsDatatable(DataTablesStructs.SentParameters parameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetInternalOutputItemsDatatable(DataTablesStructs.SentParameters parameters, Guid internalOuputId,string search);
        Task Insert(InternalOutput entity);
        Task Update(InternalOutput entity);
        Task Delete(InternalOutput entity);
        Task<InternalOutput> Get(Guid id);
    }
}
