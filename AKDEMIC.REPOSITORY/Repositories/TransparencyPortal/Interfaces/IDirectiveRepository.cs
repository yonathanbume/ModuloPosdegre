using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface IDirectiveRepository : IRepository<Directive>
    {
        Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters);
        IQueryable<Directive> GetIQueryable();
    }
}
