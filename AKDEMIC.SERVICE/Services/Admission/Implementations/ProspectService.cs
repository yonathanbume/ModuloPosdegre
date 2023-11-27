using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ProspectService : IProspectService
    {
        private readonly IProspectRepository _prospectRepository;
        public ProspectService(IProspectRepository prospectRepository)
        {
            _prospectRepository = prospectRepository;
        }

        public async Task<bool> AnyInRange(int start, int end)
        {
            return await _prospectRepository.AnyInRange(start,end);
        }

        public async Task<bool> Exists(int folderNum)
        {
            return await _prospectRepository.Exists(folderNum);
        }

        public async Task<Prospect> Get(Guid id)
        {
            return await _prospectRepository.Get(id);
        }

        public async Task<List<Prospect>> GetActiveProspects()
        {
            return await _prospectRepository.GetActiveProspects();
        }

        public async Task<DataTablesStructs.ReturnedData<Prospect>> GetDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _prospectRepository.GetDatatable(sentParameters);
        }

        public async Task Insert(Prospect prospect)
        {
            await _prospectRepository.Insert(prospect);
        }

        public async Task Update(Prospect prospect)
        {
            await _prospectRepository.Update(prospect);
        }

        public async  Task Delete(Prospect prospect)
        {
            await _prospectRepository.Delete(prospect);
        }

        public async Task<bool> CanDelete(Guid id)
        {
            return await _prospectRepository.CanDelete(id);
        }

        public async Task<Prospect> GetByNumber(int folderNum)
        {
            return await _prospectRepository.GetByNumber(folderNum);
        }
    }
}
