using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventFileService
    {
        Task Insert(EventFile entity);
        Task Update(EventFile entity);
        Task<EventFile> Get(Guid id);
        Task Delete(EventFile entity);
        Task<List<EventFile>> GetAllByEvent(Guid eventId);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid eventId);
    }
}
