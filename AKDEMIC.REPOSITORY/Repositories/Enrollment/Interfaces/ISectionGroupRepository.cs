using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ISectionGroupRepository : IRepository<SectionGroup>
    {
        Task<IEnumerable<Select2Structs.Result>> GetSectionGroupsSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionGroupDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
        Task<IEnumerable<Select2Structs.Result>> GetSectionGroupBySectionSelect2lientSide(Guid sectionId);
    }
}
