using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkCertificateRecordRepository : IRepository<WorkCertificateRecord>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkCertificateRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
