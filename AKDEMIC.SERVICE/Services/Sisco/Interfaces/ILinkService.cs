using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface ILinkService
    {
        Task InsertLink(Link link);
        Task UpdateLink(Link link);
        Task DeleteLink(Link link);
        Task<Link> GetLinkById(Guid id);
        Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllLinkDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null, byte? status = null);
        Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllNetworkDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null);
        Task<LinkTemplate> GetLinkTemplateById(Guid id);
        Task<LinkTemplate> GetLinkByIdWithOther(Guid id);
        Task<List<LinkTemplate>> GetLinkToHome();
    }
}
