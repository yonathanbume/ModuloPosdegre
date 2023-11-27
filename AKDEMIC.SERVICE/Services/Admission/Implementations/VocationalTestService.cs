using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VocationalTestService : IVocationalTestService
    {
        private readonly IVocationalTestRepository _vocationalTestRepository;

        public VocationalTestService(IVocationalTestRepository vocationalTestRepository)
        {
            _vocationalTestRepository = vocationalTestRepository;
        }

        public async Task DeleteById(Guid Id)
        {
            await _vocationalTestRepository.DeleteById(Id);
        }

        public async Task<bool> ExistAnyActive(Guid? Id)
        {
            return await _vocationalTestRepository.ExistAnyActive(Id);
        }

        public async Task<VocationalTest> Get(Guid Id)
        {
            return await _vocationalTestRepository.Get(Id);
        }

        public async Task Insert(VocationalTest vocationalTest)
        {
            await _vocationalTestRepository.Insert(vocationalTest);
        }

        public async Task Update(VocationalTest vocationalTest)
        {
            await _vocationalTestRepository.Update(vocationalTest);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _vocationalTestRepository.VocationalTestDatatable(sentParameters, searchValue);
        }
    }
}
