using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentTurn;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentTurnRepository : IRepository<EnrollmentTurn>
    {
        Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId);
        Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId, Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, 
            Guid? facultyId = null, Guid? careerId = null, int? type = null, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatableWithCredits(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, bool? isConfirmed = null, bool? isReceived = null);

        Task<object> GetStudentsDetailDatatable(Guid termId, Guid? careerId = null);
        Task<EnrollmentTurn> GetStudentTurn(Guid studentId, Guid termId);
        Task<EnrollmentTurn> GetWithData(Guid id);
        Task<EnrollmentTurn> GetByStudentIdAndTerm(Guid studentId, Guid termId);
        Task EnrollmentTurnsFixJob(Guid termid);
        Task<decimal> GetStudentCreditsWithoutTurn(Guid studentId);

        Task ValidateReceivedEnrollments();

        Task<Tuple<bool, string>> GenerateStudentTurn(Guid termId, Guid studentId);
        Task<List<EnrollmentTurnTemplate>> GetSpecialEnrollmentData(Guid termId, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}
