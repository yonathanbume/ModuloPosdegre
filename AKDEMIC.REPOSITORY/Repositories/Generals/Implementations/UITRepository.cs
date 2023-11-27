using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class UITRepository : Repository<UIT>, IUITRepository
    {
        public UITRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<UIT>> GetUITsDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<UIT, UIT>> selectPredicate = null, Expression<Func<UIT, dynamic>> orderByPredicate = null, Func<UIT, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UITs.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<UIT>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<bool> AnyUITByYear(int year)
        {
            var query = _context.UITs
                .Where(x => x.Year == year)
                .AsQueryable();

            return await query.AnyAsync();
        }

        public async Task<UIT> GetCurrentUIT()
        {
            var query = _context.UITs
                .Where(x=>x.Year == DateTime.UtcNow.Year)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<UIT>> GetUITsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<UIT, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Year);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Value);

                    break;
                default:
                    orderByPredicate = ((x) => x.Year);

                    break;
            }

            return await GetUITsDatatable(sentParameters, ExpressionHelpers.SelectUIT(), orderByPredicate, (x) => new[] { x.Value + "", x.Year + "" }, searchValue);
        }

        #endregion
    }
}
