using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserExternalProcedureService : IUserExternalProcedureService
    {
        private readonly IUserExternalProcedureRepository _userExternalProcedureRepository;

        public UserExternalProcedureService(IUserExternalProcedureRepository userExternalProcedureRepository)
        {
            _userExternalProcedureRepository = userExternalProcedureRepository;
        }

        public Task<UserExternalProcedure> GetWithIncludes(Guid id)
            => _userExternalProcedureRepository.GetWithIncludes(id);

        public async Task<int> Count()
        {
            return await _userExternalProcedureRepository.Count();
        }

        public async Task<UserExternalProcedure> Get(Guid id)
        {
            return await _userExternalProcedureRepository.Get(id);
        }

        public async Task<UserExternalProcedure> GetUserExternalProcedure(Guid id)
        {
            return await _userExternalProcedureRepository.GetUserExternalProcedure(id);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProcedures()
        {
            return await _userExternalProcedureRepository.GetUserExternalProcedures();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByDependency(Guid dependencyId)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresByDependency(dependencyId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresByExternalUser(externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId, string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresByExternalUser(externalUserId, userDependencyUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresBySearchValue(searchValue);
        }
        public async Task<IEnumerable<object>> GetUserExternalInternalProcedure(string search, int type)
            => await _userExternalProcedureRepository.GetUserExternalInternalProcedure(search, type);
        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresByUserDependencyUser(userDependencyUserId);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string searchValue = null)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresDatatableByExternalUser(sentParameters, externalUserId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string userDependencyUserId, string searchValue = null)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresDatatableByExternalUser(sentParameters, externalUserId, userDependencyUserId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string searchValue = null)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresSelect2ByDependency(requestParameters, dependencyId, searchValue);
        }

        public async Task Insert(UserExternalProcedure userExternalProcedure)
        {
            await _userExternalProcedureRepository.Insert(userExternalProcedure);
        }

        public async Task InsertUserExternalProcedure(UserExternalProcedure userExternalProcedure)
        {
            await _userExternalProcedureRepository.InsertUserExternalProcedure(userExternalProcedure);
        }
        public async Task Add(UserExternalProcedure userExternalProcedure)
            => await _userExternalProcedureRepository.Add(userExternalProcedure);
        public async Task Update(UserExternalProcedure userExternalProcedure)
        {
            await _userExternalProcedureRepository.Update(userExternalProcedure);
        }

        public async Task<UserExternalProcedure> GetByInternalProcedure(Guid internalProcedureId)
            => await _userExternalProcedureRepository.GetByInternalProcedure(internalProcedureId);











        /*
        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue2(string searchValue)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresBySearchValue2(searchValue);
        }


        public async Task<Payment> GetUserExternalProcedurePayment(Guid userExternalProcedureId)
        {
            return await _userExternalProcedureRepository.GetUserExternalProcedurePayment(userExternalProcedureId);
        }

        public async Task<bool> IsUserExternalProcedureAllowedByUserDependency(Guid userExternalProcedureId, string userId)
        {
            return await _userExternalProcedureRepository.IsUserExternalProcedureAllowedByUserDependency(userExternalProcedureId, userId);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependency(userId, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependency(userId, externalUserId, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependency(userId, beginDate, endDate, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependency(userId, beginDate, endDate, externalUserId, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresUserDependency(userId, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresUserDependency(userId, externalUserId, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresUserDependency(userId, beginDate, endDate, sentParameters);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresUserDependency(userId, beginDate, endDate, externalUserId, sentParameters);
        }

        public async Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresBySearchValue(searchValue);
        }
        */
        public async Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(Guid externalUserId, string searchValue)
        {
            return await _userExternalProcedureRepository.GetUserExternalProceduresBySearchValue(externalUserId, searchValue);
        }
        /*
        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProcedureByExternalUser(Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProcedureByExternalUser(externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependencyUser(userDependencyUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresByUserDependencyUser(userDependencyUserId, externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetActiveUserExternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByExternalUser(Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresByExternalUser(externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresByUserDependencyUser(userDependencyUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresByUserDependencyUser(userDependencyUserId, externalUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId);
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId)
        {
            return await _userExternalProcedureRepository.GetHistoricUserExternalProceduresDatatableByUserDependencyUser(sentParameters, userDependencyUserId, externalUserId);
        }*/
    }
}
