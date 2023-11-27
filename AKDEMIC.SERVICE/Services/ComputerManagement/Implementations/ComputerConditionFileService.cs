using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class ComputerConditionFileService : IComputerConditionFileService
    {
        private readonly IComputerConditionFileRepository _computerConditionFileRepository;

        public ComputerConditionFileService(IComputerConditionFileRepository computerConditionFileRepository)
        {
            _computerConditionFileRepository = computerConditionFileRepository;
        }

        public async Task Delete(ComputerConditionFile entity)
            => await _computerConditionFileRepository.Delete(entity);

        public async Task<ComputerConditionFile> Get(Guid id)
            => await _computerConditionFileRepository.Get(id);

        public async Task<IEnumerable<ComputerConditionFile>> GetAllByComputerId(Guid computerId)
            => await _computerConditionFileRepository.GetAllByComputerId(computerId);

        public async Task Insert(ComputerConditionFile entity)
            => await _computerConditionFileRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<ComputerConditionFile> entites)
            => await _computerConditionFileRepository.InsertRange(entites);
    }
}
