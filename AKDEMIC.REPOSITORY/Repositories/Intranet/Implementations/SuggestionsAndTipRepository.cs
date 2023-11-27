using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SuggestionAndTip;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SuggestionsAndTipRepository: Repository<SuggestionAndTip> , ISuggestionsAndTipRepository
    {
        public SuggestionsAndTipRepository(AkdemicContext context): base(context) {}

        public async Task<List<SuggestionAndTipTemplate>> GetSuggestionAndTipsByWelfareCategory(Guid? welfareCategoryId = null)
        {

            var query = _context.SuggestionAndTips.Include(x=>x.WelfareCategory).AsQueryable();
            if (welfareCategoryId.HasValue && welfareCategoryId != Guid.Empty)
            {
                query = query.Where(x => x.WelfareCategoryId == welfareCategoryId);
            }

            var result = await query.OrderBy(x=>x.WelfareCategory.ColorRGB).Select(x=> new SuggestionAndTipTemplate {
                Id = x.Id,
                ColorRGB = x.WelfareCategory.ColorRGB,
                Description = x.Description,
                Title = x.Title,
                GeneralTitle  = x.GeneralTitle,
                Type = x.Type
            }).ToListAsync();
            return result;
        }
    }
}
