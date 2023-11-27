using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IExternalUserRepository : IRepository<ExternalUser>
    {
        Task SaveChanges();
        Task<bool> AnyExternalUserByDni(string dni);
        Task<IEnumerable<ExternalUser>> GetExternalUsers();
        Task<DataTablesStructs.ReturnedData<object>> GetExternalUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, DateTime? startDateCreatedAt = null, DateTime? endDateCreatedAt = null, int? documentType = null, bool onlyPublicSector = false);
        Task<Select2Structs.ResponseParameters> GetExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<object> GetExternalUsersByTermSelect2(string term, byte? documentType = null);
        Select2Structs.ResponseParameters GetReniecExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null);
        Task<bool> HasAnyUserExternalProcedure(Guid externalUserId);
        Task<bool> IsDniDuplicated(string dni);
        Task<bool> IsDniDuplicated(string dni, Guid externalUserId);
        Task<Tuple<int, List<ExternalUser>>> GetExternalUsers(DataTablesStructs.SentParameters sentParameters);
        Task<List<ExternalUser>> GetExternalUsersBySearchValue(string searchValue);
        Task<object> GetExternalUsersOrStudentBySearchValue(string searchValue, int type);
        Task<ExternalUser> GetByDni(string dni);
        Task<ExternalUser> GetByDocument(int documentType, string documentNumber);
        Task<ExternalUser> GetExternalUserByUserId(string userId);
    }
}
