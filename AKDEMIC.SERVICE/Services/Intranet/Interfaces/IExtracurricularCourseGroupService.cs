using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task Insert(ExtracurricularCourseGroup extracurricularCourseGroup);
        Task Update(ExtracurricularCourseGroup extracurricularCourseGroup);
        Task DeleteById(Guid id);
        Task<ExtracurricularCourseGroup> Get(Guid id);
        Task<ExtracurricularCourseGroup> GetByCode(string code);
        Task<IEnumerable<ExtracurricularCourseGroup>> GetAll(string teacherId = null);
        Task<ExtracurricularCourseGroup> GetWithIncludes(Guid id);
    }
}
