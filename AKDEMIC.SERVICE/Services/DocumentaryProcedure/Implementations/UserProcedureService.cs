using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureService : IUserProcedureService
    {
        private readonly IUserProcedureRepository _userProcedureRepository;

        public UserProcedureService(IUserProcedureRepository userProcedureRepository)
        {
            _userProcedureRepository = userProcedureRepository;
        }

        public async Task<UserProcedure> Get(Guid id)
        {
            return await _userProcedureRepository.Get(id);
        }
        public async Task<UserProcedure> GetUserProcedure(Guid id)
        {
            return await _userProcedureRepository.GetUserProcedure(id);
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveEnrollmentReservations()
        {
            return await _userProcedureRepository.GetActiveEnrollmentReservations();
        }

        public async Task<IEnumerable<UserProcedure>> GetFinishByUserProcedure(Guid id)
            => await _userProcedureRepository.GetFinishByUserProcedure(id);
        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresBySearchValue(string searchValue)
        {
            return await _userProcedureRepository.GetActiveUserProceduresBySearchValue(searchValue);
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUser(string userId, string search)
        {
            return await _userProcedureRepository.GetActiveUserProceduresByUser(userId, search);
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string search)
        {
            return await _userProcedureRepository.GetActiveUserProceduresByUserDependencyUser(userDependencyUserId, search);
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search)
        {
            return await _userProcedureRepository.GetActiveUserProceduresByUserDependencyUser(userDependencyUserId, userId, search);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetActiveUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null, Guid? dependencyId = null, int? status = null)
            => await _userProcedureRepository.GetActiveUserProceduresByUserDependencyUserDatatable(sentParameters, userDependencyUserId, userId, search, dependencyId, status);
        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null)
            => await _userProcedureRepository.GetHistoricUserProceduresByUserDependencyUserDatatable(sentParameters, userDependencyUserId, userId, search);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDerivationUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null)
            => await _userProcedureRepository.GetDerivationUserProceduresByUserDependencyUserDatatable(sentParameters, userDependencyUserId, userId, search);
        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUser(string userId, string search)
        {
            return await _userProcedureRepository.GetHistoricUserProceduresByUser(userId, search);
        }

        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string search)
        {
            return await _userProcedureRepository.GetHistoricUserProceduresByUserDependencyUser(userDependencyUserId, search);
        }

        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search)
        {
            return await _userProcedureRepository.GetHistoricUserProceduresByUserDependencyUser(userDependencyUserId, userId, search);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProcedures()
        {
            return await _userProcedureRepository.GetUserProcedures();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByDate(DateTime start, DateTime end)
        {
            return await _userProcedureRepository.GetUserProceduresByDate(start, end);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByDependency(Guid dependencyId)
        {
            return await _userProcedureRepository.GetUserProceduresByDependency(dependencyId);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByEndDate(DateTime end)
        {
            return await _userProcedureRepository.GetUserProceduresByEndDate(end);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByProcedure(Guid procedureId)
        {
            return await _userProcedureRepository.GetUserProceduresByProcedure(procedureId);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresBySearchValue(string searchValue)
        {
            return await _userProcedureRepository.GetUserProceduresBySearchValue(searchValue);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByStartDate(DateTime start)
        {
            return await _userProcedureRepository.GetUserProceduresByStartDate(start);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId)
        {
            return await _userProcedureRepository.GetUserProceduresByUser(userId);
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId, string searchValue)
        {
            return await _userProcedureRepository.GetUserProceduresByUser(userId, searchValue);
        }

        public async Task AddAsync(UserProcedure userProcedure)
            => await _userProcedureRepository.Add(userProcedure);
        public async Task Delete(UserProcedure userProcedure) =>
            await _userProcedureRepository.Delete(userProcedure);

        public async Task Insert(UserProcedure userProcedure) =>
            await _userProcedureRepository.Insert(userProcedure);

        public async Task Update(UserProcedure userProcedure) =>
            await _userProcedureRepository.Update(userProcedure);

        public async Task<DataTablesStructs.ReturnedData<UserProcedure>> GetUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, string searchValue = null)
            => await _userProcedureRepository.GetUserProcedureDatatable(sentParameters, userId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _userProcedureRepository.GetDirectedCoursesUserProcedureDatatable(sentParameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationRequestsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
            => await _userProcedureRepository.GetEnrollmentReservationRequestsDatatable(sentParameters, facultyId, careerId, searchValue);

        public async Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null)
            => await _userProcedureRepository.GetExtemporaneousEnrollmentDatatable(user);
        public async Task<List<Guid?>> GetIdUserProcedures(List<Guid> procedureDependecies)
            => await _userProcedureRepository.GetIdUserProcedures(procedureDependecies);
        public async Task<UserProcedure> GetUserProcedureByPaymentId(Guid id)
            => await _userProcedureRepository.GetUserProcedureByPaymentId(id);

        public async Task UpdateExtemporaneousEnrollment(Guid studentId)
            => await _userProcedureRepository.UpdateExtemporaneousEnrollment(studentId);

        public async Task<List<UserProcedure>> Get(string userId, Guid termId, Guid irregularProcedureId, Guid disaprovedCourseProcedureId, Guid regularProcedureId)
        {
            return await _userProcedureRepository.Get(userId, termId, irregularProcedureId, disaprovedCourseProcedureId, regularProcedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
            => await _userProcedureRepository.GetExtemporaneousEnrollmentDatatableServerSide(sentParameters, user);

        public async Task<UserProcedure> GetUserProcedureByStaticType(string userId, int staticType)
        {
            return await _userProcedureRepository.GetUserProcedureByStaticType(userId, staticType);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetRequerimentProcedure(DataTablesStructs.SentParameters sentParameters, Guid id, int? status = null)
            => await _userProcedureRepository.GetRequerimentProcedure(sentParameters, id, status);
        public async Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureList(Guid id, int? status = null)
            => await _userProcedureRepository.GetRequerimentProcedureList(id, status);
        public async Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureListPt2(Guid id, int? status = null)
            => await _userProcedureRepository.GetRequerimentProcedureListPt2(id, status);
        public async Task<Guid?> GetNextByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId)
        => await _userProcedureRepository.GetNextByGeneratedId(generatedId, userDependencyUserId, userId, dependencyId);

        public async Task<Guid?> GetPreviousByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId)
            => await _userProcedureRepository.GetPreviousByGeneratedId(generatedId, userDependencyUserId, userId, dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, bool? completed, string search)
            => await _userProcedureRepository.GetUserProceduresDatatable(parameters, user, completed, search);

        public async Task<bool> AnyUserProcedurePending(string userId, Guid procedureId)
            => await _userProcedureRepository.AnyUserProcedurePending(userId, procedureId);

        public async Task<List<UserProcedure>> GetStudentUserProcedures(Guid studentId, Guid? termId)
            => await _userProcedureRepository.GetStudentUserProcedures(studentId, termId);

        public async Task<List<UserProcedure>> GetUserProceduresByUserId(string userId, Guid? termId, byte? status = null, Guid?  procedureId = null)
            => await _userProcedureRepository.GetUserProceduresByUserId(userId, termId, status, procedureId);

        public async Task<string> GetNextCorrelative(Guid procedureId)
            => await _userProcedureRepository.GetNextCorrelative(procedureId);
    }
}
