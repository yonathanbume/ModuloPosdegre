using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository
    {
        public CatalogItemRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DateTime> GetLastDate()
        {
            var result = await _context.CatalogItems.Where(x=>x.CreatedAt.HasValue).OrderByDescending(x => x.CreatedAt).Select(x => x.CreatedAt.Value).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogItemDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchCode = null, string searchName = null)
        {
            Expression<Func<CatalogItem, dynamic>> orderByPredicate = null;
            Func<CatalogItem, object[]> searchValuePredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CodeUnitMeasurement;
                    break;
                case "3":
                    orderByPredicate = (x) => x.UnitMeasurement;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Type;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.CatalogItems.AsNoTracking();


            if (type.HasValue)
            {
                query = query.Where(q => q.Type == type);
            }

            if (!string.IsNullOrEmpty(searchCode))
                query = query
                    .Where(x => x.Code.ToUpper().Contains(searchCode));
            if (!string.IsNullOrEmpty(searchName))
                query = query
                    .Where(x => x.Description.ToUpper().Contains(searchName));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    description = x.Description,
                    codeUnitMeasurement = x.CodeUnitMeasurement,
                    unitMeasurement = x.UnitMeasurement,
                    type = ConstantHelpers.ECONOMICMANAGEMENT.TYPECATALOG.VALUES[x.Type]
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

        public SelectList GetTypeCatalog()
        {
            var result = new SelectList(ConstantHelpers.ECONOMICMANAGEMENT.TYPECATALOG.VALUES, "Key", "Value");
            return result;
        }

        public async Task<bool> AnyByCode(string code)
            => await _context.CatalogItems.AnyAsync(x => x.Code == code);

        public async Task<object> GetCatalogItemsToInternalOuput(Guid dependencyId)
        {
            var items = await _context.CatalogItems
                .Where(x => x.UserRequirementItems.Any(y => y.UserRequirement.Requirement.DependencyId == dependencyId && 
                y.UserRequirement.OrderId.HasValue && (y.UserRequirement.Order.Status == ConstantHelpers.ORDERS.STATUS.INTERNSHIP || y.UserRequirement.Order.Status == ConstantHelpers.ORDERS.STATUS.FINALIZED)))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Description,
                    x.CodeUnitMeasurement,
                    x.UnitMeasurement,
                    quantityReceived = _context.ReceivedOrders.Where(y => y.UserRequirement.UserRequirementItems.Any(z => z.CatalogItemId == x.Id) && 
                    y.UserRequirement.Requirement.DependencyId == dependencyId && y.UserRequirement.OrderId.HasValue && (y.UserRequirement.Order.Status == ConstantHelpers.ORDERS.STATUS.INTERNSHIP || y.UserRequirement.Order.Status == ConstantHelpers.ORDERS.STATUS.FINALIZED)).Sum(y => y.QuantityReceived),
                    heritages = x.InternalOutputItems.Sum(z=>z.Quantity),
                })
                .ToListAsync();

            return items;
        }

    }
}
