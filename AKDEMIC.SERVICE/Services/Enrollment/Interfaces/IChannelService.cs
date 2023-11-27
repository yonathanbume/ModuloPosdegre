using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IChannelService
    {
        Task DeleteById(Guid id);
        Task Insert(Channel channel);
        Task Update(Channel channel);
        Task<Channel> Get(Guid id);
        Task<IEnumerable<Channel>> GetAll();
        Task<List<Channel>> GetAllOnlyWithCareers();
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
    }
}
