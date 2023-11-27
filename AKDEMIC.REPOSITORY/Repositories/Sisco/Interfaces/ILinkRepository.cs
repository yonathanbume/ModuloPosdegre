using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface ILinkRepository : IRepository<Link>
    {
        Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllLinkDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null, byte? status = null);
        Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllNetworkDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null);
        Task<LinkTemplate> GetLinkById(Guid id);
        Task<LinkTemplate> GetLinkByIdWithOther(Guid id);

        Task<List<LinkTemplate>> GetLinkToHome();
    }
}
