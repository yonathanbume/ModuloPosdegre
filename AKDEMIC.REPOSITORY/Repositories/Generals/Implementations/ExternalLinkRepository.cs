using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class ExternalLinkRepository: Repository<ExternalLink>, IExternalLinkRepository
    {
        public ExternalLinkRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, string searchValue = null)
        {
            Expression<Func<ExternalLink, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Type;
                    break;
            }

            var query = _context.ExternalLinks
                .AsNoTracking();

            if (type != null)
            {
                query = query.Where(x => x.Type == type);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
            .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            .Select(x => new
            {
                id = x.Id,
                description = x.Description,
                type = ConstantHelpers.EXTERNAL_LINK.TYPE.VALUES.ContainsKey(x.Type) ?
                    ConstantHelpers.EXTERNAL_LINK.TYPE.VALUES[x.Type] : "-",
            })
            .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
