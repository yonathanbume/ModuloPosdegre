using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencySubMenuService
    {
        Task Insert(TransparencySubMenu entity);
        Task Update(TransparencySubMenu entity);
        Task Delete(TransparencySubMenu entity);
        Task<TransparencySubMenu> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencySubMenuDatatable(DataTablesStructs.SentParameters parameters);

    }
}
