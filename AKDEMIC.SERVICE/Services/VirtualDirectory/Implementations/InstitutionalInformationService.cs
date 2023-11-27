using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces;
using AKDEMIC.SERVICE.Services.VirtualDirectory.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VirtualDirectory.Implementations
{
    public class InstitutionalInformationService : IInstitutionalInformationService
    {
        private readonly IInstitutionalInformationRepository _institutionalInformationRepository;

        public InstitutionalInformationService(IInstitutionalInformationRepository institutionalInformationRepository)
        {
            _institutionalInformationRepository = institutionalInformationRepository;
        }

        public async Task Delete(InstitutionalInformation entity)
            => await _institutionalInformationRepository.Delete(entity);

        public async Task<InstitutionalInformation> Get(Guid id)
            => await _institutionalInformationRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalInformationDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, Guid? dependencyId = null, DateTime? publishDate = null, string search = null)
            => await _institutionalInformationRepository.GetInstitutionalInformationDatatable(sentParameters, type, dependencyId, publishDate, search);

        public async Task Insert(InstitutionalInformation entity)
            => await _institutionalInformationRepository.Insert(entity);

        public async Task Update(InstitutionalInformation entity)
            => await _institutionalInformationRepository.Update(entity);
    }
}
