﻿using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyAdvertisingRepository : IRepository<TransparencyAdvertising>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters);
    }
}
