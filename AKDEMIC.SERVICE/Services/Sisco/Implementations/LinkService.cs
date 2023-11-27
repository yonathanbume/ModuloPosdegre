using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        public LinkService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task InsertLink(Link link) =>
            await _linkRepository.Insert(link);
        public async Task UpdateLink(Link link) =>
            await _linkRepository.Update(link);
        public async Task DeleteLink(Link link) =>
            await _linkRepository.Delete(link);
        public async Task<Link> GetLinkById(Guid id) =>
            await _linkRepository.Get(id);
        public async Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllLinkDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null, byte? status = null) =>
            await _linkRepository.GetAllLinkDatatable(sentParameters, type, title, status);
        public async Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllNetworkDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null) =>
            await _linkRepository.GetAllNetworkDatatable(sentParameters, title, status);
        public async Task<LinkTemplate> GetLinkTemplateById(Guid id) =>
            await _linkRepository.GetLinkById(id);

        public async Task<LinkTemplate> GetLinkByIdWithOther(Guid id) =>
            await _linkRepository.GetLinkByIdWithOther(id);

        public async Task<List<LinkTemplate>> GetLinkToHome() =>
            await _linkRepository.GetLinkToHome();
    }
}
