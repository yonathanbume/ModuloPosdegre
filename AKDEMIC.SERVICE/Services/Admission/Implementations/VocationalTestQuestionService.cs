using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VocationalTestQuestionService : IVocationalTestQuestionService
    {
        private readonly IVocationalTestQuestionRepository _vocationalTestQuestionRepository;

        public VocationalTestQuestionService(IVocationalTestQuestionRepository vocationalTestQuestionRepository)
        {
            _vocationalTestQuestionRepository = vocationalTestQuestionRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _vocationalTestQuestionRepository.DeleteById(id);
        }

        public async Task<VocationalTestQuestion> Get(Guid vocacionalTestQuestionId)
        {
            return await _vocationalTestQuestionRepository.Get(vocacionalTestQuestionId);
        }

        public async Task<List<VocationalTestQuestion>> GetActiveVocationalTestQuestions()
        {
            return await _vocationalTestQuestionRepository.GetActiveVocationalTestQuestions();
        }

        public async Task Insert(VocationalTestQuestion vocationalTestQuestion)
        {
            await _vocationalTestQuestionRepository.Insert(vocationalTestQuestion);
        }

        public async Task Update(VocationalTestQuestion vocationalTestQuestion)
        {
            await _vocationalTestQuestionRepository.Update(vocationalTestQuestion);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestQuestionDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestId, string searchValue = null)
        {
            return await _vocationalTestQuestionRepository.VocationalTestQuestionDatatable(sentParameters, vocationalTestId, searchValue);
        }
    }
}
