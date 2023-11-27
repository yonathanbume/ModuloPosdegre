using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class AgreementTypeRepository : Repository<AgreementType>, IAgreementTypeRepository
    {
        public AgreementTypeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<AgreementType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    break;
            }

            var query = _context.AgreementTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    code = x.Code
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetSelect2ClientSide()
        {
            var data = await _context.AgreementTypes
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Code
                }).ToListAsync();

            return data;
        }
    }
}
