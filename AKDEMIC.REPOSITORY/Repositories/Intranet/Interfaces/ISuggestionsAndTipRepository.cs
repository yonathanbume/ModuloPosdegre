using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SuggestionAndTip;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISuggestionsAndTipRepository : IRepository<SuggestionAndTip>
    {
        Task<List<SuggestionAndTipTemplate>> GetSuggestionAndTipsByWelfareCategory(Guid? welfareCategoryId = null);
    }
}
