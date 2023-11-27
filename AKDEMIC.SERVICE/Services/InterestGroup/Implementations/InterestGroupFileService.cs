using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupFileService : IInterestGroupFileService
    {
        private readonly IInterestGroupFileRepository _interestGroupFileRepository;

        public InterestGroupFileService(IInterestGroupFileRepository interestGroupFileRepository)
        {
            _interestGroupFileRepository = interestGroupFileRepository;
        }

        public async Task Delete(InterestGroupFile entity)
            => await _interestGroupFileRepository.Delete(entity);

        public async Task<InterestGroupFile> Get(Guid id)
            => await _interestGroupFileRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<InterestGroupFile>> GetInterestGroupFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid interestGroupId, string searchValue = null)
            => await _interestGroupFileRepository.GetInterestGroupFileDatatable(sentParameters, interestGroupId, searchValue);

        public async Task Insert(InterestGroupFile entity)
            => await _interestGroupFileRepository.Insert(entity);

        public async Task Update(InterestGroupFile entity)
            => await _interestGroupFileRepository.Update(entity);
    }
}
