using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface ISectionSiscoRepository : IRepository<SectionSisco>
    {
        Task<DataTablesStructs.ReturnedData<SectionSiscoTemplate>> GetAllSectionDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null);
        Task<SectionSiscoTemplate> GetAvailableOrdersAndListSequenceOrder();
        Task<SectionSiscoTemplate> GetSectionById(Guid id);
        Task<List<SectionSiscoTemplate>> GetSectionToHome();

    }
}
