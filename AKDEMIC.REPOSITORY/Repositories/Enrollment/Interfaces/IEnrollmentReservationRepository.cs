using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentReservation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentReservationRepository : IRepository<EnrollmentReservation>
    {
        Task<List<EnrollmentReservation>> GetEnrollmentReservations();
        Task<IEnumerable<EnrollmentReservation>> GetEnrollmentReservationsByStudent(Guid studentId, Guid? termId = null);
        Task<bool> ValidateStudentReservationExist(Guid termId, Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<ReservationExcelTemplate>> GetEnrollmentReservationsExcel(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}
