using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureCategoryRepository : Repository<ProcedureCategory>, IProcedureCategoryRepository
    {
        public ProcedureCategoryRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<ProcedureCategory, ProcedureCategory>> selectPredicate = null, Expression<Func<ProcedureCategory, dynamic>> orderByPredicate = null, Func<ProcedureCategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureCategories.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProcedureCategory>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ProcedureCategory, Select2Structs.Result>> selectPredicate = null, Func<ProcedureCategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureCategories.AsNoTracking();
            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetProcedureProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ProcedureCategory, Select2Structs.Result>> selectPredicate = null, Func<ProcedureCategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureCategories
                .Where(x => _context.Procedures.Any(y => y.ProcedureCategoryId == x.Id))
                .AsNoTracking();

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<ProcedureCategory>> GetProcedureCategories()
        {
            var query = _context.ProcedureCategories;

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ProcedureCategory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureCategoriesDatatable(sentParameters, ExpressionHelpers.SelectProcedureCategory(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetProcedureCategoriesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetProcedureProcedureCategoriesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion







        public async Task<IEnumerable<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureCategory, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.ProcedureCategories
                .Where(x => searchValuePredicate != null ? searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2) : true)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return await query.ToListAsync();
        }
    }
}
