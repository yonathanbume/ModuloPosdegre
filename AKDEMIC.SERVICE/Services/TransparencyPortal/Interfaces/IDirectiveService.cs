using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IDirectiveService
    {
        Task Insert(Directive directive);
        Task<Directive> Get(Guid id);
        Task Update(Directive directive);
        Task DeleteById(Guid id);
        Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters);
        Task<IEnumerable<Directive>> GetAll();
        IQueryable<Directive> GetIQueryable();
    }
}
