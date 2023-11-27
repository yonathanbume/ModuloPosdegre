using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class SectionSiscoService : ISectionSiscoService
    {
        private readonly ISectionSiscoRepository _sectionRepository;

        public SectionSiscoService(ISectionSiscoRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task InsertSection(SectionSisco section) =>
            await _sectionRepository.Insert(section);

        public async Task UpdateSection(SectionSisco section) =>
            await _sectionRepository.Update(section);

        public async Task DeleteSection(SectionSisco section) =>
            await _sectionRepository.Delete(section);

        public async Task<SectionSisco> GetSectionById(Guid id) =>
            await _sectionRepository.Get(id);
        public async Task<IEnumerable<SectionSisco>> GetAllSection() =>
            await _sectionRepository.GetAll();
        public async Task<DataTablesStructs.ReturnedData<SectionSiscoTemplate>> GetAllSectionDatatable(DataTablesStructs.SentParameters sentParameters, string title, byte status) =>
            await _sectionRepository.GetAllSectionDatatable(sentParameters, title, status);
        public async Task<SectionSiscoTemplate> GetAvailableOrdersAndListSequenceOrder() =>
            await _sectionRepository.GetAvailableOrdersAndListSequenceOrder();

        public async Task<SectionSiscoTemplate> GetSectionTemplateById(Guid id) =>
            await _sectionRepository.GetSectionById(id);

        public async Task<List<SectionSiscoTemplate>> GetSectionToHome() =>
            await _sectionRepository.GetSectionToHome();
    }
}
