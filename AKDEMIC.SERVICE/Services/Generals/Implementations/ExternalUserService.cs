using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{

    public class ExternalUserService : IExternalUserService
    {
        private readonly IExternalUserRepository _externalUserRepository;

        public ExternalUserService(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<bool> AnyExternalUserByDni(string dni)
        {
            return await _externalUserRepository.AnyExternalUserByDni(dni);
        }

        public async Task<ExternalUser> Get(Guid id)
        {
            return await _externalUserRepository.Get(id);
        }

        public async Task<IEnumerable<ExternalUser>> GetAll()
        {
            return await _externalUserRepository.GetAll();
        }

        public async Task<IEnumerable<ExternalUser>> GetExternalUsers()
        {
            return await _externalUserRepository.GetExternalUsers();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, DateTime? startDateCreatedAt = null, DateTime? endDateCreatedAt = null, int? documentType = null, bool onlyPublicSector = false)
        {
            return await _externalUserRepository.GetExternalUsersDatatable(sentParameters, searchValue, startDateCreatedAt, endDateCreatedAt, documentType, onlyPublicSector);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _externalUserRepository.GetExternalUsersSelect2(requestParameters, searchValue);
        }

        public async Task<object> GetExternalUsersByTermSelect2(string term, byte? documentType = null)
            => await _externalUserRepository.GetExternalUsersByTermSelect2(term, documentType);

        public Select2Structs.ResponseParameters GetReniecExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return _externalUserRepository.GetReniecExternalUsersSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _externalUserRepository.GetUserExternalProcedureExternalUsersSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
        {
            return await _externalUserRepository.GetUserExternalProcedureExternalUsersSelect2(requestParameters, userId, searchValue);
        }

        public async Task Delete(ExternalUser externalUser)
        {
            await _externalUserRepository.Delete(externalUser);
        }

        public async Task Insert(ExternalUser externalUser)
        {
            await _externalUserRepository.Insert(externalUser);
        }

        public async Task Update(ExternalUser externalUser)
        {
            await _externalUserRepository.Update(externalUser);
        }

        public async Task Add(ExternalUser externalUser)
            => await _externalUserRepository.Add(externalUser);

        public async Task<bool> IsDniDuplicated(string dni)
        {
            return await _externalUserRepository.IsDniDuplicated(dni);
        }

        public async Task<bool> IsDniDuplicated(string dni, Guid externalUserId)
        {
            return await _externalUserRepository.IsDniDuplicated(dni, externalUserId);
        }

        public async Task<bool> HasAnyUserExternalProcedure(Guid externalUserId)
        {
            return await _externalUserRepository.HasAnyUserExternalProcedure(externalUserId);
        }

        public async Task<Tuple<int, List<ExternalUser>>> GetExternalUsers(DataTablesStructs.SentParameters sentParameters)
        {
            return await _externalUserRepository.GetExternalUsers(sentParameters);
        }

        public async Task<List<ExternalUser>> GetExternalUsersBySearchValue(string searchValue)
        {
            return await _externalUserRepository.GetExternalUsersBySearchValue(searchValue);
        }
        public async Task<object> GetExternalUsersOrStudentBySearchValue(string searchValue, int type)
            => await _externalUserRepository.GetExternalUsersOrStudentBySearchValue(searchValue, type);

        public async Task<ExternalUser> GetByDni(string dni)
            => await _externalUserRepository.GetByDni(dni);

        public Task<ExternalUser> GetByDocument(int documentType, string documentNumber)
            => _externalUserRepository.GetByDocument(documentType, documentNumber);

        public async Task<ExternalUser> GetExternalUserByUserId(string userId)
            => await _externalUserRepository.GetExternalUserByUserId(userId);

        public Task SaveChanges()
            => _externalUserRepository.SaveChanges();
    }
}
