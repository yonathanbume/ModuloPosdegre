using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupRepository : IRepository<ExtracurricularCourseGroup>
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<ExtracurricularCourseGroup>> GetAll(string teacherId = null);
        Task<ExtracurricularCourseGroup> GetByCode(string code);
        Task<ExtracurricularCourseGroup> GetWithIncludes(Guid id);
    }
}
