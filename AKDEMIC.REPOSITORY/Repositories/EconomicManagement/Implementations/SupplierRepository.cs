using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<Supplier, dynamic>> GetSuppliersDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.RUC);
                case "2":
                    return ((x) => x.User.FullName);
                default:
                    return ((x) => x.Name);
            }
        }

        private Func<Supplier, string[]> GetSuppliersDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name + "",
                x.RUC + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, Expression<Func<Supplier, Supplier>> selectPredicate = null, Expression<Func<Supplier, dynamic>> orderByPredicate = null, Func<Supplier, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Suppliers
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<bool> AnySupplierByName(string name)
        {
            var query = _context.Suppliers.Where(x => x.Name == name);

            return await query.AnyAsync();
        }

        public async Task<bool> AnySupplierByName(string name, string ruc)
        {
            var query = _context.Suppliers.Where(x => x.Name == name || x.RUC == ruc);

            return await query.AnyAsync();
        }

        public async Task<bool> AnySupplierByRUC(string ruc)
        {
            var query = _context.Suppliers.Where(x => x.RUC == ruc);

            return await query.AnyAsync();
        }

        public async Task<bool> AnySupplierByNameDistint(string name, string ruc, Guid id)
            => await _context.Suppliers.Where(x => x.Id != id && (x.Name == name || x.RUC == ruc)).AnyAsync();

        public async Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetSuppliersDatatable(sentParameters, null, ExpressionHelpers.SelectSupplier(), GetSuppliersDatatableOrderByPredicate(sentParameters), GetSuppliersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await GetSuppliersDatatable(sentParameters, userId, ExpressionHelpers.SelectSupplier(), GetSuppliersDatatableOrderByPredicate(sentParameters), GetSuppliersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<object> GetSuppliers()
        {
            var result = await _context.Suppliers.Include(x => x.User)
                .Select(x => new
                {
                    x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }
        #endregion
    }
}
