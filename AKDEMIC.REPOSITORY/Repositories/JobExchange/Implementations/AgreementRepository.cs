using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class AgreementRepository : Repository<Agreement>, IAgreementRepository
    {
        public AgreementRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAgreementDatatable(DataTablesStructs.SentParameters sentParameters, int state, bool isActive, string searchValue = null)
        {
            Expression<Func<Agreement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Year);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }
            var query = _context.Agreements
                 .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                 .AsNoTracking();

            var today = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Contains(searchValue) || x.Institution.Contains(searchValue));
            }

            switch (state)
            {
                case 0:
                    break;
                case 1:
                    query = query.Where(x => x.IsUndefined || (x.StartDate <= today && today <= x.EndDate)).AsQueryable();
                    break;
                case 2:
                    query = query.Where(x => x.EndDate < today).AsQueryable();
                    break;
            }

            if (isActive)
            {
                var add30 = today.AddDays(+30);
                query = query.Where(x => !x.IsUndefined && today <= x.EndDate.Value && x.EndDate.Value <= add30).AsQueryable();
            }

            Expression<Func<Agreement, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                x.Name,
                x.Institution,
                EndDateSituation =  !x.IsUndefined ? x.EndDate.ToLocalDateFormat() : "Indefinido",
                State =  x.IsUndefined ? true : (x.StartDate <= today && today <= x.EndDate)
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetAgreementsSelect2()
        {
            var result = await _context.Agreements.ToListAsync();
            result.Insert(0, new Agreement { Id = Guid.Empty, Name = "Sin convenio" });

            var finalResult =  result
            .Select(x => new 
            {
                x.Id,
                Text = x.Name
            }).ToList();

            
            return finalResult;
        }
    }
}
