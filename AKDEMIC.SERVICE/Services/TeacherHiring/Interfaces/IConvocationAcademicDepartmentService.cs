using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationAcademicDepartmentService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search);
        Task Insert(ConvocationAcademicDeparment entity);
        Task Update(ConvocationAcademicDeparment entity);
        Task Delete(ConvocationAcademicDeparment entity);
        Task<bool> AnyByAcademicDepartmentId(Guid convocationId, Guid academicDepartmentId, Guid? ignoreId = null);
        Task<ConvocationAcademicDeparment> Get(Guid id);
    }
}
