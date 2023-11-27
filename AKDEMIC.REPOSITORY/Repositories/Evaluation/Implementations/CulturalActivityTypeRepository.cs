using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class CulturalActivityTypeRepository : Repository<CulturalActivityType>, ICulturalActivityTypeRepository
    {
        public CulturalActivityTypeRepository(AkdemicContext context) :base(context) { }

        public async Task<bool> AnyByTypeId(Guid typeId)
            => await _context.CulturalActivities.AnyAsync(x => x.TypeId == typeId);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.CulturalActivityTypes.AnyAsync(x => x.Name == name && x.Id == ignoredId);

        public async Task<DataTablesStructs.ReturnedData<CulturalActivityType>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<CulturalActivityType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.CulturalActivityTypes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<CulturalActivityType, CulturalActivityType>> selectPredicate = (x) => new CulturalActivityType
            {
                Id = x.Id,
                Name = x.Name,
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetTypeSelect2ClientSide()
        {
            var result = await _context.CulturalActivityTypes
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }
    }
}
