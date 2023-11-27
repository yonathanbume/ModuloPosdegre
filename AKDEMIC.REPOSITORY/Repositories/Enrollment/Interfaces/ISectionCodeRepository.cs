using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ISectionCodeRepository : IRepository<SectionCode>
    {
        Task<object> GetFakeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetSectionCodeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
