using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ISectionGroupService
    {
        Task<IEnumerable<Select2Structs.Result>> GetSectionGroupsSelect2ClientSide();
        Task Insert(SectionGroup entity);
        Task Update(SectionGroup entity);
        Task Delete(SectionGroup entity);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<SectionGroup> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionGroupDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
        Task<IEnumerable<Select2Structs.Result>> GetSectionGroupBySectionSelect2lientSide(Guid sectionId);
    }
}
