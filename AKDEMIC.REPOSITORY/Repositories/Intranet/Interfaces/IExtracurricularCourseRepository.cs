using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularCourseRepository : IRepository<ExtracurricularCourse>
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<ExtracurricularCourse> GetByCode(string code);
        Task<IEnumerable<Select2Structs.Result>> GetExtracurricularCoursesSelect2ClientSide();

    }
}
