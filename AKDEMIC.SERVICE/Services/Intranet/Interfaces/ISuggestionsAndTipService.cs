using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SuggestionAndTip;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISuggestionsAndTipService
    {
        Task<SuggestionAndTip> Get(Guid id);
        Task Update(SuggestionAndTip suggestionsAndTip);
        Task Delete(SuggestionAndTip suggestionsAndTip);
        Task<List<SuggestionAndTipTemplate>> GetSuggestionAndTipsByWelfareCategory(Guid? welfareCategoryId = null);
        Task Insert(SuggestionAndTip suggestionAndTip);
    }
}
