using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencySubMenuRepository : IRepository<TransparencySubMenu>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencySubMenuDatatable(DataTablesStructs.SentParameters parameters);
    }
}
