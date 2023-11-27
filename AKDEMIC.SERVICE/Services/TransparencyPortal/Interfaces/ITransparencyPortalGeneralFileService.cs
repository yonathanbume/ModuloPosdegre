using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalGeneralFileService
    {
        Task Insert(TransparencyPortalGeneralFile file);
        Task<TransparencyPortalGeneralFile> Get(Guid id);
        Task Update(TransparencyPortalGeneralFile file);
        Task DeleteById(Guid id);
        Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<IEnumerable<TransparencyPortalGeneralFile>> GetAll();
        Task<IEnumerable<TransparencyPortalGeneralFile>> GetAll(Guid generalId);
        Task DeleteRange(IEnumerable<TransparencyPortalGeneralFile> files);
    }
}
