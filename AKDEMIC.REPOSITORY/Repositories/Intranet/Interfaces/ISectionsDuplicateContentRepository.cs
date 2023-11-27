using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISectionsDuplicateContentRepository : IRepository<SectionsDuplicateContent>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsDuplicateContentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId);
        Task<SectionsDuplicateContent> GetBySectionAandB(Guid sectionAid, Guid sectionBid);
        Task<bool> AnySectionASectionB(Guid sectionAid, Guid sectionBid);
    }
}
