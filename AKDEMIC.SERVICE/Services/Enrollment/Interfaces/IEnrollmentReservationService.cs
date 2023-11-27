using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentReservation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentReservationService
    {
        Task<EnrollmentReservation> Get(Guid id);
        Task Update(EnrollmentReservation entity);
        Task<List<EnrollmentReservation>> GetEnrollmentReservations();
        Task Delete(EnrollmentReservation enrollmentReservation);
        Task Insert(EnrollmentReservation enrollmentReservation);
        Task<IEnumerable<EnrollmentReservation>> GetEnrollmentReservationsByStudent(Guid studentId, Guid? termId = null);
        Task<bool> ValidateStudentReservationExist(Guid termId, Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<ReservationExcelTemplate>> GetEnrollmentReservationsExcel(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}
