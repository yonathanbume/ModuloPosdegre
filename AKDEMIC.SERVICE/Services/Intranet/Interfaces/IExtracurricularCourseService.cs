using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularCourseService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<ExtracurricularCourse> Get(Guid id);
        Task<IEnumerable<ExtracurricularCourse>> GetAll();
        Task Insert(ExtracurricularCourse extracurricularCourse);
        Task Update(ExtracurricularCourse extracurricularCourse);
        Task DeleteById(Guid id);
        Task<ExtracurricularCourse> GetByCode(string code);
        Task<IEnumerable<Select2Structs.Result>> GetExtracurricularCoursesSelect2ClientSide();
    }
}
