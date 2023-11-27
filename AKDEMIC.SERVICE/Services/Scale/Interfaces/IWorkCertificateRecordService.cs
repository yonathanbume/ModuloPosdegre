using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkCertificateRecordService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkCertificateRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task Insert(WorkCertificateRecord entity);
    }
}
