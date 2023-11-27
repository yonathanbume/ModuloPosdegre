using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureService
    {
        Task<int> Count();
        Task<UserExternalProcedure> Get(Guid id);
        Task<UserExternalProcedure> GetWithIncludes(Guid id);
        Task<UserExternalProcedure> GetUserExternalProcedure(Guid id);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProcedures();
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByDependency(Guid dependencyId);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId, string userDependencyUserId);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue);
        Task<IEnumerable<object>> GetUserExternalInternalProcedure(string search, int type);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByUserDependencyUser(string userDependencyUserId);
        Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string userDependencyUserId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null);
        Task Insert(UserExternalProcedure userExternalProcedure);
        Task InsertUserExternalProcedure(UserExternalProcedure userExternalProcedure);
        Task Add(UserExternalProcedure userExternalProcedure);
        Task Update(UserExternalProcedure userExternalProcedure);
        Task<UserExternalProcedure> GetByInternalProcedure(Guid internalProcedureId);












        /*
        Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProcedureByExternalUser(Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId);
        Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId);
        Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByExternalUser(Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId);
        Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId);
        Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByDependency(Guid dependencyId);
        Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue2(string searchValue);

        Task<Payment> GetUserExternalProcedurePayment(Guid userExternalProcedureId);
        Task<bool> IsUserExternalProcedureAllowedByUserDependency(Guid userExternalProcedureId, string userId);

        Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters);

        Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters);
        Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters);

        Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue);*/
        Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(Guid externalUserId, string searchValue);
    }
}
