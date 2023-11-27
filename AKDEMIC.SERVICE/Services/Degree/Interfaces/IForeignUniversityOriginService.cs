using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Interfaces
{
    public interface IForeignUniversityOriginService
    {
        Task<ForeignUniversityOrigin> Get(Guid id);
        Task Insert(ForeignUniversityOrigin foreignUniversityOrigin);
        Task Delete(ForeignUniversityOrigin foreignUniversityOrigin);
        Task Update(ForeignUniversityOrigin foreignUniversityOrigin);

        Task<DataTablesStructs.ReturnedData<object>> GetForeignUniveristyOriginDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSelect2();
    }
}
