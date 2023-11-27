using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class UniversityAuthorityService : IUniversityAuthorityService
    {
        private readonly IUniversityAuthorityRepository _universityAuthorityRepository;

        public UniversityAuthorityService(IUniversityAuthorityRepository universityAuthorityRepository)
        {
            _universityAuthorityRepository = universityAuthorityRepository;
        }
        public async Task<bool> ExistAuthorityType(int type, Guid? universityAuthorityId)
        {
            return await _universityAuthorityRepository.ExistAuthorityType(type, universityAuthorityId);
        }

        public async Task<UniversityAuthority> Get(Guid id)
        {
            return await _universityAuthorityRepository.Get(id);
        }

        public async Task<List<UniversityAuthority>> GetUniversityAuthoritiesList()
        {
            return await _universityAuthorityRepository.GetUniversityAuthoritiesList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthority(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _universityAuthorityRepository.GetUniversityAuthority(sentParameters, searchValue);
        }

        public async Task Insert(UniversityAuthority newEntity)
        {
            await _universityAuthorityRepository.Insert(newEntity);
        }

        public async Task Update(UniversityAuthority universityAuthority)
        {
            await _universityAuthorityRepository.Update(universityAuthority);
        }
        public async Task DeleteById(Guid id)
        {
            await _universityAuthorityRepository.DeleteById(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthorityHistory(Guid id)
        {
            return await _universityAuthorityRepository.GetUniversityAuthorityHistory(id);
        }
    }
}
