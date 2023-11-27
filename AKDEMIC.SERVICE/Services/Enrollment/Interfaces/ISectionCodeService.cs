using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ISectionCodeService
    {
        Task<object> GetFakeSelect2();
        Task Insert(SectionCode sectionCode);
        Task Update(SectionCode sectionCode);
        Task Delete(SectionCode sectionCode);
        Task<SectionCode> Get(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetSectionCodeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<SectionCode>>GetAll();
    }
}