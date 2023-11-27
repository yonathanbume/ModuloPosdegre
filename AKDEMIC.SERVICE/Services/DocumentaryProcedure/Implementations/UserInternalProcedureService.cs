using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserInternalProcedureService : IUserInternalProcedureService
    {
        private readonly IUserInternalProcedureRepository _userInternalProcedureRepository;

        public UserInternalProcedureService(IUserInternalProcedureRepository userInternalProcedureRepository)
        {
            _userInternalProcedureRepository = userInternalProcedureRepository;
        }

        public async Task<int> Count()
        {
            return await _userInternalProcedureRepository.Count();
        }

        public async Task<UserInternalProcedure> Get(Guid id)
        {
            return await _userInternalProcedureRepository.Get(id);
        }

        public async Task<UserInternalProcedure> GetUserInternalProcedure(Guid id)
        {
            return await _userInternalProcedureRepository.GetUserInternalProcedure(id);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, userId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, userId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, userId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            return await _userInternalProcedureRepository.GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, dependencyId, userId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetPastProcedure(DataTablesStructs.SentParameters sentParameters, string search)
            => await _userInternalProcedureRepository.GetPastProcedure(sentParameters, search);
        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyClaimId, status, search, procedureFolderId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, dependencyClaimId, status, search, procedureFolderId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null) 
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, userId, dependencyClaimId, status, search, procedureFolderId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, userId, dependencyClaimId, status , search, procedureFolderId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, userId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            return await _userInternalProcedureRepository.GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, dependencyId, userId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        {
            return await _userInternalProcedureRepository.GetChildUserInternalProceduresByInternalProcedure(internalProcedureId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            return await _userInternalProcedureRepository.GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, userId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresByInternalProcedureUser(internalProcedureUserId, dependencyId, userId, dependencyClaimId, search);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, userId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            return await _userInternalProcedureRepository.GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(sentParameters, internalProcedureUserId, dependencyId, userId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresByInternalProcedure(internalProcedureId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        }
        //public async Task<DataTablesStructs.ReturnedData<object>> GetParentUserInternalProceduresByInternalProcedureDatatble(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid? dependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        //    => await _userInternalProcedureRepository.GetParentUserInternalProceduresByInternalProcedureDatatble(sentParameters, internalProcedureId, dependencyId, startCreatedAt, endCreatedAt);
        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, internalProcedureDependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetParentUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid dependencyId, string search = null, int? status = null)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresByUserDependencyUser(userDependencyUserId, dependencyId, search, status);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresByUserDependencyUser(userDependencyUserId, internalProcedureDependencyId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresByUserDependencyUser(userDependencyUserId, internalProcedureUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresByUserDependencyUser(userDependencyUserId, internalProcedureDependencyId, internalProcedureUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, internalProcedureDependencyId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, internalProcedureUserId, dependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId)
        {
            return await _userInternalProcedureRepository.GetRecievedUserInternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, internalProcedureDependencyId, internalProcedureUserId, dependencyId);
        }

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        //{
        //    return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByInternalProcedure(internalProcedureId);
        //}

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId)
        //{
        //    return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);
        //}

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        //{
        //    return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        //}

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt, Guid userProcedureId)
        {
            return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByInternalProcedure(internalProcedureId, startCreatedAt, endCreatedAt, userProcedureId);
        }

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId)
        //{
        //    return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureId);
        //}

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, internalProcedureDependencyId);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            return await _userInternalProcedureRepository.GetRelatedUserInternalProceduresDatatableByInternalProcedure(sentParameters, internalProcedureId, startCreatedAt, endCreatedAt);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProcedures()
        {
            return await _userInternalProcedureRepository.GetUserInternalProcedures();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDate(DateTime start, DateTime end)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByDate(start, end);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDependency(Guid dependencyId, byte? status = null)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByDependency(dependencyId, status);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByEndDate(end);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end, int searchTree)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByEndDate(end, searchTree);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByInternalProcedureSearchTree(int searchTree)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByInternalProcedureSearchTree(searchTree);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByStartDate(start);
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start, int searchTree)
        {
            return await _userInternalProcedureRepository.GetUserInternalProceduresByStartDate(start, searchTree);
        }

        public async Task Delete(UserInternalProcedure userInternalProcedure) =>
            await _userInternalProcedureRepository.Delete(userInternalProcedure);

        public async Task Insert(UserInternalProcedure userInternalProcedure) =>
            await _userInternalProcedureRepository.Insert(userInternalProcedure);

        public async Task InsertRange(IEnumerable<UserInternalProcedure> userInternalProcedures) =>
            await _userInternalProcedureRepository.InsertRange(userInternalProcedures);

        public async Task Update(UserInternalProcedure userInternalProcedure) =>
            await _userInternalProcedureRepository.Update(userInternalProcedure);

        public async Task<Dictionary<string,int>> GetUserInternalProcedureByDependencies()
        {
            return await _userInternalProcedureRepository.GetUserInternalProcedureByDependencies();
        }

        public async Task<object> GetUserInternalProcedureReportByMonthChart()
        {
            return await _userInternalProcedureRepository.GetUserInternalProcedureReportByMonthChart();
        }
        public async Task<Guid> GetInternalProcedureAnswer(Guid uid, string userId)
            => await _userInternalProcedureRepository.GetInternalProcedureAnswer(uid, userId);
        public async Task<UserInternalProcedure> GetByUserExternalProcedure(Guid userExternalProcedureId)
            => await _userInternalProcedureRepository.GetByUserExternalProcedure(userExternalProcedureId);

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByUserInternalProcedure(Guid userInternalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
            => await _userInternalProcedureRepository.GetRelatedUserInternalProceduresByUserInternalProcedure(userInternalProcedureId, startCreatedAt, endCreatedAt);   
    }
}
