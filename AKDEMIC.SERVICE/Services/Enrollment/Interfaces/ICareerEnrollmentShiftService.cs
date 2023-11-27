using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICareerEnrollmentShiftService
    {
        Task<object> GetDataDatatableClientSide(Guid enrollmentShiftId, ClaimsPrincipal user = null);
        Task UpdateInEnrollmentShift(Guid enrollmentShiftId, List<CareerEnrollmentShift> careerEnrollmentShifts);
        Task Insert(CareerEnrollmentShift careerEnrollmentShift);
        Task Update(CareerEnrollmentShift careerEnrollmentShift);
        Task<CareerEnrollmentShift> Get(Guid id);
        Task<CareerEnrollmentShift> Get(Guid termId, Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetCareerShiftDatatable(DataTablesStructs.SentParameters sentParameters, Guid enrollmentShiftId, ClaimsPrincipal user = null, string search = null);
    }
}
