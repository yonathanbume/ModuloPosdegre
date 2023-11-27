using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VocationalTestAnswerCareerService : IVocationalTestAnswerCareerService
    {
        private readonly IVocationalTestAnswerCareerRepository _vocationalTestAnswerCareerRepository;

        public VocationalTestAnswerCareerService(IVocationalTestAnswerCareerRepository vocationalTestAnswerCareerRepository)
        {
            _vocationalTestAnswerCareerRepository = vocationalTestAnswerCareerRepository;
        }
        public async Task DeleteById(Guid id)
        {
            await _vocationalTestAnswerCareerRepository.DeleteById(id);
        }

        public async Task DeleteRange(List<VocationalTestAnswerCareer> vocationalTestAnswerCareers)
        {
            await _vocationalTestAnswerCareerRepository.DeleteRange(vocationalTestAnswerCareers);
        }

        public async Task<VocationalTestAnswerCareer> Get(Guid vocationalTestAnswerCareerId)
        {
            return await _vocationalTestAnswerCareerRepository.Get(vocationalTestAnswerCareerId);
        }

        public async Task<List<Guid>> GetCareersByAnswers(Guid vocationalTestAnswerCareerId)
        {
            return await _vocationalTestAnswerCareerRepository.GetCareersByAnswers(vocationalTestAnswerCareerId);
        }

        public async Task<VocationalTestAnswerCareer> GetVocationalTestAnswerCareerFirstOrDefault(Guid vocationalTestAnswerId)
        {
            return await _vocationalTestAnswerCareerRepository.GetVocationalTestAnswerCareerFirstOrDefault(vocationalTestAnswerId);
        }

        public async Task<List<VocationalTestAnswerCareer>> GetVocationalTestAnswerCareersFiltered(Guid vocationalTestAnswerId)
        {
            return await _vocationalTestAnswerCareerRepository.GetVocationalTestAnswerCareersFiltered(vocationalTestAnswerId);
        }

        public async Task Insert(VocationalTestAnswerCareer vocationalTestAnswerCareer)
        {
            await _vocationalTestAnswerCareerRepository.Insert(vocationalTestAnswerCareer);
        }

        public async Task Update(VocationalTestAnswerCareer vocationalTestAnswerCareer)
        {
            await _vocationalTestAnswerCareerRepository.Update(vocationalTestAnswerCareer);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue = null)
        {
            return await _vocationalTestAnswerCareerRepository.VocationalTestAnswerCareerDatatable(sentParameters, vocationalTestQuestionId, searchValue);
        }
    }
}
