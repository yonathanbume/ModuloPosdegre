using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ISessionRecordFilesRepository:IRepository<SessionRecordFile>
    {
        Task<List<SessionRecordFile>> GetBySessionRecordId(Guid id);
        Task<object> GetById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId, DateTime? startDate, DateTime? endDate, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetFilesDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId = null, DateTime? startDate = null, DateTime? endDate = null, string search = null);
    }
}
