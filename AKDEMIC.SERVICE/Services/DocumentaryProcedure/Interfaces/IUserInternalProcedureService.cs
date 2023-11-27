using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserInternalProcedureService
    {
        Task<int> Count();
        Task<UserInternalProcedure> Get(Guid id);
        Task<UserInternalProcedure> GetUserInternalProcedure(Guid id);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId);
        Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetPastProcedure(DataTablesStructs.SentParameters sentParameters, string search);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, int? status = null, string search  =null, Guid? procedureFolderId = null);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId);
        Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId);
        Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId);
        Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId);
        Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid dependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        //Task<DataTablesStructs.ReturnedData<object>> GetParentUserInternalProceduresByInternalProcedureDatatble(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid? dependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid dependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid userDependencyId, string search = null, int? status = null);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string internalProcedureUserId, Guid dependencyId);
        Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt, Guid userProcedureId);

        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        //Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProcedures();
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDate(DateTime start, DateTime end);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDependency(Guid dependencyId, byte? status = null);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end, int searchTree);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByInternalProcedureSearchTree(int searchTree);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start);
        Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start, int searchTree);
        Task Delete(UserInternalProcedure userInternalProcedure);
        Task Insert(UserInternalProcedure userInternalProcedure);
        Task InsertRange(IEnumerable<UserInternalProcedure> userInternalProcedure);
        Task Update(UserInternalProcedure userInternalProcedure);
        Task<Dictionary<string,int>> GetUserInternalProcedureByDependencies();
        Task<object> GetUserInternalProcedureReportByMonthChart();
        Task<Guid> GetInternalProcedureAnswer(Guid uid, string userId);
        Task<UserInternalProcedure> GetByUserExternalProcedure(Guid userExternalProcedureId);
        Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByUserInternalProcedure(Guid userInternalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt);
    }
}
