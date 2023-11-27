using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISectionsDuplicateContentService
    {
        Task InsertSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent);
        Task UpdateSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent);
        Task DeleteSectionsDuplicateContent(SectionsDuplicateContent sectionsDuplicateContent);
        Task<IEnumerable<SectionsDuplicateContent>> GetAllSectionsDuplicateContents();
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsDuplicateContentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId);
        Task<SectionsDuplicateContent> GetBySectionAandB(Guid sectionAid, Guid sectionBid);
        Task<bool> AnySectionASectionB(Guid sectionAid, Guid sectionBid);
    }
}
