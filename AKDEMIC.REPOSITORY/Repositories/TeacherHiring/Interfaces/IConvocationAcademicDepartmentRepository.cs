using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationAcademicDepartmentRepository : IRepository<ConvocationAcademicDeparment>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search);
        Task<bool> AnyByAcademicDepartmentId(Guid convocationId, Guid academicDepartmentId , Guid? ignoreId = null);
    }
}
