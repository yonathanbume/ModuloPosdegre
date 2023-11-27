using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryChannelService
    {
        Task<PreuniversitaryChannel> Get(Guid id);
        Task<IEnumerable<PreuniversitaryChannel>> GetAll();
        //Task<DataTablesStructs.ReturnedData<object>> GetGroupsDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null);
        Task Insert(PreuniversitaryChannel entity);
        Task Update(PreuniversitaryChannel entity);
        Task DeleteById(PreuniversitaryChannel entity);
    }
}
