using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermManagerService: IApplicationTermManagerService
    {
        private readonly IApplicationTermManagerRepository _applicationTermManagerRepository;
        public ApplicationTermManagerService(IApplicationTermManagerRepository applicationTermManagerRepository)
        {
            _applicationTermManagerRepository = applicationTermManagerRepository;
        }

        public async Task<bool> AnyByApplicationTerm(Guid id, string userId)
        {
            return await _applicationTermManagerRepository.AnyByApplicationTerm(id, userId);
        }

        public async Task DeleteById(Guid id)
        {
            await _applicationTermManagerRepository.DeleteById(id);
        }

        public async Task<ApplicationTermManager> Get(Guid id)
        {
            return await _applicationTermManagerRepository.Get(id);
        }

        public async Task<List<ApplicationTermManager>> GetByApplicationTermId(Guid id)
        {
            return await _applicationTermManagerRepository.GetByApplicationTermId(id);
        }

        public async Task<DataTablesStructs.ReturnedData<ApplicationTermManager>> GetByApplicationTermIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue)
        {
            return await _applicationTermManagerRepository.GetByApplicationTermIdDataTable(sentParameters, id, searchValue);
        }

        public async Task Insert(ApplicationTermManager applicationTermManager)
        {
            await _applicationTermManagerRepository.Insert(applicationTermManager);
        }

        public async Task Update(ApplicationTermManager applicationTermManager)
        {
            await _applicationTermManagerRepository.Update(applicationTermManager);
        }
    }
}
