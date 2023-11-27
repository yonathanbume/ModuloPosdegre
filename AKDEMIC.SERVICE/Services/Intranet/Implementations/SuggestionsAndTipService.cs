using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SuggestionAndTip;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SuggestionsAndTipService : ISuggestionsAndTipService
    {
        private readonly ISuggestionsAndTipRepository _suggestionsAndTipRepository;
        public SuggestionsAndTipService(ISuggestionsAndTipRepository suggestionsAndTipRepository)
        {
            _suggestionsAndTipRepository = suggestionsAndTipRepository;
        }

        public async Task Delete(SuggestionAndTip suggestionsAndTip)
        {
            await _suggestionsAndTipRepository.Delete(suggestionsAndTip);
        }

        public async Task<SuggestionAndTip> Get(Guid id)
        {
            return await _suggestionsAndTipRepository.Get(id);
        }

        public async Task<List<SuggestionAndTipTemplate>> GetSuggestionAndTipsByWelfareCategory(Guid? welfareCategoryId = null)
        {
            return await _suggestionsAndTipRepository.GetSuggestionAndTipsByWelfareCategory(welfareCategoryId);
        }

        public async Task Insert(SuggestionAndTip suggestionAndTip)
        {
            await _suggestionsAndTipRepository.Insert(suggestionAndTip);
        }

        public async Task Update(SuggestionAndTip suggestionsAndTip)
        {
            await _suggestionsAndTipRepository.Update(suggestionsAndTip);
        }
    }
}
