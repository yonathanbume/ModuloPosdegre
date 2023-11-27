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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<Order, dynamic>> GetOrdersDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Title);
                case "1":
                    return ((x) => x.Cost);
                case "2":
                    return ((x) => x.EndDate);
                case "3":
                    return ((x) => x.StartDate);
                case "4":
                    return ((x) => x.Status);
                default:
                    return ((x) => x.Title);
            }
        }

        private Func<Order, string[]> GetOrdersDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Title + "",
                x.Cost + "",
                x.EndDate + "",
                x.StartDate + "",
                x.Status + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? supplierId, DateTime? endDate = null, DateTime? startDate = null, Expression<Func<Order, Order>> selectPredicate = null, Expression<Func<Order, dynamic>> orderByPredicate = null, Func<Order, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Orders
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (supplierId != null)
            {
                query = query.Where(x => _context.UserRequirements.Include(y => y.Requirement).Where(y => y.OrderId == x.Id && y.Requirement.SupplierId == supplierId).Any());
            }

            if (endDate != null)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }

            if (startDate != null)
            {
                query = query.Where(x => x.StartDate <= startDate);
            }

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<int> CountBySupplier(Guid supplierId)
        {
            return await _context.Orders.Where(x => _context.UserRequirements.Include(y => y.Requirement).Where(y => y.OrderId == x.Id && y.Requirement.SupplierId == supplierId).Any()).CountAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersBySupplier(Guid supplierId, string searchValue = null)
        {
            var query = _context.Orders
                .WhereSearchValue((x) => new[]
                {
                    x.Title + "",
                    x.Cost + "",
                    x.EndDate + "",
                    x.StartDate + "",
                    x.Status + ""
                }, searchValue)
                .Where(x => _context.UserRequirements.Include(y => y.Requirement).Where(y => y.OrderId == x.Id && y.Requirement.SupplierId == supplierId).Any())
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetOrdersDatatable(sentParameters, null, null, null, ExpressionHelpers.SelectOrder(), GetOrdersDatatableOrderByPredicate(sentParameters), GetOrdersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByEndDate(DataTablesStructs.SentParameters sentParameters, DateTime endDate, string searchValue = null)
        {
            return await GetOrdersDatatable(sentParameters, null, endDate, null, ExpressionHelpers.SelectOrder(), GetOrdersDatatableOrderByPredicate(sentParameters), GetOrdersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByStartDate(DataTablesStructs.SentParameters sentParameters, DateTime startDate, string searchValue = null)
        {
            return await GetOrdersDatatable(sentParameters, null, null, startDate, ExpressionHelpers.SelectOrder(), GetOrdersDatatableOrderByPredicate(sentParameters), GetOrdersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableBySupplier(DataTablesStructs.SentParameters sentParameters, Guid supplierId, string searchValue = null)
        {
            return await GetOrdersDatatable(sentParameters, supplierId, null, null, ExpressionHelpers.SelectOrder(), GetOrdersDatatableOrderByPredicate(sentParameters), GetOrdersDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<Order> GetByUserRequirementOn(Guid userRequirementId)
        {
            var query = _context.Orders.Include(x => x.OrderChanges)
                    .Where(x => x.UserRequirements.Any(y => y.Id == userRequirementId));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Order> GetByNumber(int orderNumber)
            => await _context.Orders.Where(x => x.Number == orderNumber).FirstOrDefaultAsync();
        public async Task<bool> ValidateOrderNumber(Guid id, int orderNumber)
        {
            var query = _context.Orders
                .Where(x => x.Id != id);

            return await query.AnyAsync(x => x.Number == orderNumber);
        }

        public async Task<Order> GetWithData(Guid id)
            => await _context.Orders
                        .Include(x => x.UserRequirements).ThenInclude(x => x.Requirement).ThenInclude(x => x.Supplier)
            .Where(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Order, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StartDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "4":
                    orderByPredicate = (x) => x.EndDate;
                    break;
                default:
                    orderByPredicate = (x) => x.Title;
                    break;
            }


            var query = _context.Orders
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = _context.Orders.AsQueryable();
            

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => (x.Title.Contains(search)));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                   x.Id,
                   code = x.Code,
                   x.Title,
                   startDate = x.StartDate.ToLocalDateFormat(),
                   endDate = x.EndDate.ToLocalDateFormat(),
                   status = ConstantHelpers.ORDERS.STATUS.VALUES[x.Status],
                   statusId = x.Status
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListNoDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Order, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StartDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "4":
                    orderByPredicate = (x) => x.EndDate;
                    break;
                default:
                    orderByPredicate = (x) => x.Title;
                    break;
            }


            var query = _context.Orders
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = _context.Orders.Where(x => x.Status == ConstantHelpers.ORDERS.STATUS.PENDING).AsQueryable();


            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => (x.Title.Contains(search)));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat()
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
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetObtainDateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValueSupplier, string searchValueOrderCode, string searchValue)
        {
            Expression<Func<Order, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Number;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "3":
                    orderByPredicate = (x) => x.StartDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.EndDate;
                    break;
                default:
                    orderByPredicate = (x) => x.Number;
                    break;
            }


            var query = _context.Orders
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(searchValueSupplier))
                query = query.Where(x => x.UserRequirements.Any(s => s.Requirement.SupplierId.HasValue && s.Requirement.Supplier.RUC.Contains(searchValueSupplier)));

            if (!string.IsNullOrEmpty(searchValueOrderCode))
                query = query.Where(x => x.Number.ToString().Contains(searchValueOrderCode));

            //if (!string.IsNullOrEmpty(searchValue))
            //    query = query.Where(x => (x.Code).Contains(searchValue));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    x.Id,
                    Code = x.Number.ToString(),
                    Title = x.Title,
                    starDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat()
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
       
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId)
        {
            Expression<Func<Order, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "1":
                    orderByPredicate = (x) => x.UserRequirements.Any(x => x.Requirement.DependencyId == dependencyId);
                    break;
                case "2":
                    orderByPredicate = (x) => x.StartDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.EndDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.Title;
                    break;
            }


            var query = _context.Orders
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = _context.Orders.AsQueryable();

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
                query = query.Where(x => x.UserRequirements.Any(x => x.Requirement.DependencyId == dependencyId));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    order = x.Title,
                    dependency = x.UserRequirements.Select(i => i.Requirement.User.UserDependencies.Select(y => y.Dependency.Name).FirstOrDefault()).FirstOrDefault(),
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    status = ConstantHelpers.ORDERS.STATUS.VALUES[x.Status],
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
        public async Task<bool> AnyAsync(int number)
            => await _context.Orders.AnyAsync(s => s.Number == number);

        public async Task<List<Order>> GetAllByDependency(Guid dependencyId)
        {
            var orders = await _context.Orders
                .Where(x => x.UserRequirements.Any(y => 
                    y.Requirement.User.UserDependencies.Any(z => z.DependencyId == dependencyId)))
                .ToListAsync();

            return orders;
        }


        #endregion
    }
}
