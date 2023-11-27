using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces;
using AKDEMIC.SERVICE.Services.VirtualDirectory.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VirtualDirectory.Implementations
{
    public class DirectoryDependencyService : IDirectoryDependencyService
    {
        private readonly IDirectoryDependencyRepository _directoryDependencyRepository;

        public DirectoryDependencyService(IDirectoryDependencyRepository directoryDependencyRepository)
        {
            _directoryDependencyRepository = directoryDependencyRepository;
        }

        public async Task Delete(DirectoryDependency entity)
            => await _directoryDependencyRepository.Delete(entity);

        public async Task<DirectoryDependency> Get(Guid id)
            => await _directoryDependencyRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPeopleInChargeDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependecyId, string search)
            => await _directoryDependencyRepository.GetPeopleInChargeDatatable(sentParameters, dependecyId, search);

        public async Task<object> GetPeopleInChargeToDirectory(PaginationParameter paginationParameters, byte filterType, string searchValue)
            => await _directoryDependencyRepository.GetPeopleInChargeToDirectory(paginationParameters,filterType,searchValue);

        public async Task<bool> HasPersonInCharge(Guid dependencyId, byte charge, Guid? id = null)
            => await _directoryDependencyRepository.HasPersonInCharge(dependencyId, charge, id);

        public async Task Insert(DirectoryDependency entity)
            => await _directoryDependencyRepository.Insert(entity);

        public async Task Update(DirectoryDependency entity)
            => await _directoryDependencyRepository.Update(entity);
    }
}
