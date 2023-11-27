using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SectionsDuplicateContentService : ISectionsDuplicateContentService
    {
        private readonly ISectionsDuplicateContentRepository _sectionsDuplicateContentRepository;

        public SectionsDuplicateContentService(ISectionsDuplicateContentRepository sectionsDuplicateContentRepository)
        {
            _sectionsDuplicateContentRepository = sectionsDuplicateContentRepository;
        }

        public async Task InsertSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent) =>
            await _sectionsDuplicateContentRepository.Insert(sectionsDuplicateContent);

        public async Task UpdateSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent) =>
            await _sectionsDuplicateContentRepository.Update(sectionsDuplicateContent);

        public async Task DeleteSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent) =>
            await _sectionsDuplicateContentRepository.Delete(sectionsDuplicateContent);

        public async Task<SectionsDuplicateContent> GetSectionsDuplicateContentById(Guid id) =>
            await _sectionsDuplicateContentRepository.Get(id);

        public async Task<IEnumerable<SectionsDuplicateContent>> GetAllSectionsDuplicateContents() =>
            await _sectionsDuplicateContentRepository.GetAll();
        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsDuplicateContentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId)
            => await _sectionsDuplicateContentRepository.GetSectionsDuplicateContentDatatable(sentParameters, termId, careerId);
        public async Task<SectionsDuplicateContent> GetBySectionAandB(Guid sectionAid, Guid sectionBid)
            => await _sectionsDuplicateContentRepository.GetBySectionAandB(sectionAid, sectionBid);
        public async Task<bool> AnySectionASectionB(Guid sectionAid, Guid sectionBid)
            => await _sectionsDuplicateContentRepository.AnySectionASectionB(sectionAid, sectionBid);
    }
}
