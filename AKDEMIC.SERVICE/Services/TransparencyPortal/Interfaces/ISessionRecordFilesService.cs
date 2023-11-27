using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ISessionRecordFilesService
    {
        Task Insert(SessionRecordFile regulation);
        Task<SessionRecordFile> Get(Guid id);
        Task Update(SessionRecordFile regulation);
        Task DeleteById(Guid id);
        Task<List<SessionRecordFile>> GetBySessionRecordId(Guid id);
        Task<object> GetById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId, DateTime? startDate, DateTime? endDate, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetFilesDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId = null, DateTime? startDate = null, DateTime? endDate = null, string search = null);
    }
}
