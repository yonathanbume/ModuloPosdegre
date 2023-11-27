using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
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
    public class ProcedureSubcategoryRepository : Repository<ProcedureSubcategory>, IProcedureSubcategoryRepository
    {
        public ProcedureSubcategoryRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? procedureCategoryId, Expression<Func<ProcedureSubcategory, ProcedureSubcategory>> selectPredicate = null, Expression<Func<ProcedureSubcategory, dynamic>> orderByPredicate = null, Func<ProcedureSubcategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureSubcategories.AsNoTracking();

            if (procedureCategoryId != null)
            {
                query = query.Where(x => x.ProcedureCategoryId == procedureCategoryId);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProcedureSubcategory>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, Guid? procedureCategoryId, Expression<Func<ProcedureSubcategory, Select2Structs.Result>> selectPredicate = null, Func<ProcedureSubcategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureSubcategories.AsNoTracking();

            if (procedureCategoryId != null)
            {
                query = query.Where(x => x.ProcedureCategoryId == procedureCategoryId);
            }

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

        private async Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, Guid? procedureCategoryId, Expression<Func<ProcedureSubcategory, Select2Structs.Result>> selectPredicate = null, Func<ProcedureSubcategory, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureSubcategories
                .Where(x => _context.Procedures.Any(y => y.ProcedureSubcategoryId == x.Id))
                .AsNoTracking();

            if (procedureCategoryId != null)
            {
                query = query.Where(x => x.ProcedureCategoryId == procedureCategoryId);
            }

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

        public async Task<bool> AnyProcedureSubcategory(Guid id)
        {
            var query = _context.ProcedureSubcategories.Where(x => x.Id == id);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyProcedureSubcategory(Guid id, Guid procedureCategoryId)
        {
            var query = _context.ProcedureSubcategories.Where(x => x.Id == id && x.ProcedureCategoryId == procedureCategoryId);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyProcedureSubcategoryByCategory(Guid procedureCategoryId)
        {
            var query = _context.ProcedureSubcategories.Where(x => x.ProcedureCategoryId == procedureCategoryId);

            return await query.AnyAsync();
        }

        public async Task<ProcedureSubcategory> GetProcedureSubcategory(Guid id)
        {
            var query = _context.ProcedureSubcategories
                .Where(x => x.Id == id)
                .SelectProcedureSubcategory();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategories()
        {
            var query = _context.ProcedureSubcategories.SelectProcedureSubcategory();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesByCategory(Guid categoryId)
        {
            var query = _context.ProcedureSubcategories
                .Where(x => x.ProcedureCategoryId == categoryId)
                .SelectProcedureSubcategory()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ProcedureSubcategory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureSubcategoriesDatatable(sentParameters, null, ExpressionHelpers.SelectProcedureSubcategory(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null)
        {
            Expression<Func<ProcedureSubcategory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.ProcedureCategory.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureSubcategoriesDatatable(sentParameters, procedureCategoryId, ExpressionHelpers.SelectProcedureSubcategory(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetProcedureSubcategoriesSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await GetProcedureSubcategoriesSelect2(requestParameters, procedureCategoryId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetProcedureProcedureSubcategoriesSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await GetProcedureProcedureSubcategoriesSelect2(requestParameters, procedureCategoryId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion













        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.ProcedureSubcategories
                .Where(x => searchValuePredicate != null ? searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2) : true)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectProcedureSubcategory();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByCategory(DataTablesStructs.SentParameters sentParameters, Guid categoryId, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.ProcedureSubcategories
                .Where(x => x.ProcedureCategoryId == categoryId)
                .Where(x => searchValuePredicate != null ? searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2) : true)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectProcedureSubcategory();

            return await query.ToListAsync();
        }
    }
}
