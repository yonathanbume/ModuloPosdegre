using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface ISectionSiscoService
    {
        Task InsertSection(SectionSisco section);
        Task UpdateSection(SectionSisco section);
        Task DeleteSection(SectionSisco section);
        Task<SectionSisco> GetSectionById(Guid id);
        Task<IEnumerable<SectionSisco>> GetAllSection();
        Task<DataTablesStructs.ReturnedData<SectionSiscoTemplate>> GetAllSectionDatatable(DataTablesStructs.SentParameters sentParameters, string title, byte status);
        Task<SectionSiscoTemplate> GetAvailableOrdersAndListSequenceOrder();
        Task<SectionSiscoTemplate> GetSectionTemplateById(Guid id);
        Task<List<SectionSiscoTemplate>> GetSectionToHome();
    }
}
