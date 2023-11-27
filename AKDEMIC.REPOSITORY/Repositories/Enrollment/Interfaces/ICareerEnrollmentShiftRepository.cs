using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICareerEnrollmentShiftRepository : IRepository<CareerEnrollmentShift>
    {
        Task<CareerEnrollmentShift> Get(Guid termId, Guid careerId);
        Task<object> GetDataDatatableClientSide(Guid enrollmentShiftId, ClaimsPrincipal user = null);
        Task<IEnumerable<CareerEnrollmentShift>> GetAllByEnrollmentShift(Guid enrollmentShiftId);
        Task<DataTablesStructs.ReturnedData<object>> GetCareerShiftDatatable(DataTablesStructs.SentParameters sentParameters, Guid enrollmentShiftId, ClaimsPrincipal user = null, string search = null);
    }
}
