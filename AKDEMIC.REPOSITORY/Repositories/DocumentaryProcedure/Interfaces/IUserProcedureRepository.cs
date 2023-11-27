using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRepository : IRepository<UserProcedure>
    {
        Task<string> GetNextCorrelative(Guid procedureId);
        Task<UserProcedure> GetUserProcedure(Guid id);
        Task<List<UserProcedure>> GetStudentUserProcedures(Guid studentId, Guid? termId);
        Task<IEnumerable<UserProcedure>> GetActiveEnrollmentReservations();
        Task<IEnumerable<UserProcedure>> GetActiveUserProceduresBySearchValue(string searchValue);
        Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUser(string userId, string search);
        Task<IEnumerable<UserProcedure>> GetFinishByUserProcedure(Guid id);
        Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string search);
        Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetActiveUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null, Guid? dependencyId = null, int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDerivationUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null);
        Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUser(string userId, string search);
        Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string search);
        Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search);
        Task<IEnumerable<UserProcedure>> GetUserProcedures();
        Task<IEnumerable<UserProcedure>> GetUserProceduresByDate(DateTime start, DateTime end);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByDependency(Guid dependencyId);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByEndDate(DateTime end);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByProcedure(Guid procedureId);
        Task<IEnumerable<UserProcedure>> GetUserProceduresBySearchValue(string searchValue);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByStartDate(DateTime start);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId);
        Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId, string searchValue);
        Task<DataTablesStructs.ReturnedData<UserProcedure>> GetUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationRequestsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null);

        Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null);
        Task<List<Guid?>> GetIdUserProcedures(List<Guid> procedureDependecies);
        Task<UserProcedure> GetUserProcedureByPaymentId(Guid id);
        Task UpdateExtemporaneousEnrollment(Guid studentId);
        Task<List<UserProcedure>> Get(string userId, Guid termId, Guid irregularProcedureId, Guid disaprovedCourseProcedureId, Guid regularProcedureId);
        Task<bool> AnyUserProcedurePending(string userId, Guid procedureId);
        Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null);
        Task<UserProcedure> GetUserProcedureByStaticType(string userId, int staticType);
        Task<Guid?> GetNextByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId);
        Task<Guid?> GetPreviousByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId);
        Task<DataTablesStructs.ReturnedData<object>> GetRequerimentProcedure(DataTablesStructs.SentParameters sentParameters, Guid id, int? status = null);
        Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureList(Guid id, int? status = null);
        Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureListPt2(Guid id, int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, bool? completed, string search);
        Task<List<UserProcedure>> GetUserProceduresByUserId(string userId, Guid? termId, byte? status = null, Guid? procedureId = null);
    }
}
