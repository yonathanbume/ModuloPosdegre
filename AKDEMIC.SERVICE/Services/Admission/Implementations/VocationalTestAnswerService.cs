using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VocationalTestAnswerService : IVocationalTestAnswerService
    {
        private readonly IVocationalTestAnswerRepository _vocationalTestAnswerRepository;

        public VocationalTestAnswerService(IVocationalTestAnswerRepository vocationalTestAnswerRepository)
        {
            _vocationalTestAnswerRepository = vocationalTestAnswerRepository;
        }
        public async Task DeleteById(Guid id)
        {
            await _vocationalTestAnswerRepository.DeleteById(id);
        }

        public async Task<VocationalTestAnswer> Get(Guid id)
        {
            return await _vocationalTestAnswerRepository.Get(id);
        }

        public async Task Insert(VocationalTestAnswer vocationalTestAnswer)
        {
            await _vocationalTestAnswerRepository.Insert(vocationalTestAnswer);
        }

        public async Task Update(VocationalTestAnswer vocationalTestAnswer)
        {
            await _vocationalTestAnswerRepository.Update(vocationalTestAnswer);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue)
        {
            return await _vocationalTestAnswerRepository.VocationalTestAnswersDatatable(sentParameters, vocationalTestQuestionId, searchValue);
        }
    }
}
