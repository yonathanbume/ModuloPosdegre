using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class RequirementSupplierRepository : Repository<RequirementSupplier>, IRequirementSupplierRepository
    {
        public RequirementSupplierRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<RequirementSupplier, dynamic>> GetRequirementSuppliersDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.RequirementId);
                case "1":
                    return ((x) => x.RequirementId);
                default:
                    return ((x) => x.RequirementId);
            }
        }

        private Func<RequirementSupplier, string[]> GetRequirementSuppliersDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Requirement.Description + "",
                x.Supplier.Name + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<RequirementSupplier>> GetRequirementSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<RequirementSupplier, RequirementSupplier>> selectPredicate = null, Expression<Func<RequirementSupplier, dynamic>> orderByPredicate = null, Func<RequirementSupplier, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RequirementSuppliers
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<RequirementSupplier>> GetRequirementSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetRequirementSuppliersDatatable(sentParameters, ExpressionHelpers.SelectRequirementSupplier(), GetRequirementSuppliersDatatableOrderByPredicate(sentParameters), GetRequirementSuppliersDatatableSearchValuePredicate(), searchValue);
        }

        public IQueryable<RequirementSupplier> GetQueryWithData(Guid id)
            => _context.RequirementSuppliers.Include(x => x.Supplier).Where(x => x.RequirementId == id).AsQueryable();

        public async Task<object> GetSelectSupplier(Guid id)
        {
            var result = await _context.RequirementSuppliers.Include(x => x.Supplier)
                .Where(x => x.RequirementId == id)
                .Select(x => new
                {
                    x.Id,
                    text = x.Supplier.Name
                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplierDatatable(DataTablesStructs.SentParameters sentParameters, Guid urquid, string search)
        {
            Expression<Func<RequirementSupplier, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Supplier.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Supplier.RUC;
                    break;
                default:
                    orderByPredicate = (x) => x.Supplier.Name;
                    break;
            }


            var query = _context.RequirementSuppliers.Include(x => x.Supplier).Where(x => x.RequirementId == urquid)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Supplier.Name.Contains(search) ||
                                         x.Supplier.RUC.Contains(search));

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    x.Id,
                    name = x.Supplier.Name,
                    ruc = x.Supplier.RUC
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnySupplierId(Guid supplierId, Guid requerimentId)
            => await _context.RequirementSuppliers.AnyAsync(x => x.SupplierId == supplierId && x.RequirementId == requerimentId);
        #endregion
    }
}
