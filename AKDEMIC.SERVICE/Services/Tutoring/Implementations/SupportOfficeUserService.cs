using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class SupportOfficeUserService : ISupportOfficeUserService
    {
        private readonly ISupportOfficeUserRepository _supportOfficeUserRepository;

        public SupportOfficeUserService(ISupportOfficeUserRepository supportOfficeUserRepository)
        {
            _supportOfficeUserRepository = supportOfficeUserRepository;
        }

        public async Task Delete(SupportOfficeUser supportOfficeUser)
            => await _supportOfficeUserRepository.Delete(supportOfficeUser);

        public async Task DeleteById(string id)
            => await _supportOfficeUserRepository.DeleteById(id);

        public async Task<SupportOfficeUser> Get(string supportOfficeUserId)
            => await _supportOfficeUserRepository.Get(supportOfficeUserId);

        public async Task<IEnumerable<SupportOfficeUser>> GetAll()
            => await _supportOfficeUserRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<SupportOfficeUser>> GetSupportOfficeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null)
            => await _supportOfficeUserRepository.GetSupportOfficeUsersDatatable(sentParameters, searchValue, supportOfficeId);

        public async Task Insert(SupportOfficeUser supportOfficeUser)
            => await _supportOfficeUserRepository.Insert(supportOfficeUser);

        public async Task Update(SupportOfficeUser supportOfficeUser)
            => await _supportOfficeUserRepository.Update(supportOfficeUser);

        public async Task<SupportOfficeUser> GetByUser(string userId)
            => await _supportOfficeUserRepository.GetByUser(userId);

        public Task<SupportOffice> GetSupportOfficeByUser(string userId)
            => _supportOfficeUserRepository.GetSupportOfficeByUser(userId);

        public Task<SupportOfficeUser> Add(SupportOfficeUser supportOfficeUser)
            => _supportOfficeUserRepository.Add(supportOfficeUser);
    }
}
