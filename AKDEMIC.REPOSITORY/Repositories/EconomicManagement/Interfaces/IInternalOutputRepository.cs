using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IInternalOutputRepository : IRepository<InternalOutput>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInternalOuputsDatatable(DataTablesStructs.SentParameters parameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetInternalOutputItemsDatatable(DataTablesStructs.SentParameters parameters, Guid internalOuputId ,string search);
    }
}
