using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class UserRequirementItemRepository : Repository<UserRequirementItem>, IUserRequirementItemRepository
    {
        public UserRequirementItemRepository(AkdemicContext akdemicContext) : base(akdemicContext) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserRequirementItmDatatable(DataTablesStructs.SentParameters sentParameters, Guid itemId, string search = null)
        {
            Expression<Func<UserRequirementItem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CatalogItem.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CatalogItem.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CatalogItem.CodeUnitMeasurement;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CatalogItem.UnitMeasurement;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CatalogItem.Type;
                    break;
                default:
                    orderByPredicate = (x) => x.CatalogItem.Code;
                    break;
            }

            var query = _context.UserRequirementItems.Include(x => x.CatalogItem).Where(x => x.CatalogItemId == itemId).AsNoTracking();

            var recordsFiltered = await query.CountAsync();


            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.CatalogItem.Code,
                    description = x.CatalogItem.Description,
                    codeUnitMeasurement = x.CatalogItem.CodeUnitMeasurement,
                    unitMeasurement = x.CatalogItem.UnitMeasurement,
                    type = x.CatalogItem.Type
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

        public async Task<object> GetAllUserRequerimentItemsByUserRequerimentId(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<UserRequirementItem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CatalogItem.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CatalogItem.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CatalogItem.CodeUnitMeasurement;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CatalogItem.UnitMeasurement;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CatalogItem.Type;
                    break;
                default:
                    orderByPredicate = (x) => x.CatalogItem.Code;
                    break;
            }

            var query = _context.UserRequirementItems.Where(x => x.UserRequirementId == id).AsNoTracking();

            var recordsFiltered = await query.CountAsync();


            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.CatalogItem.Id,
                    code = x.CatalogItem.Code,
                    description = x.CatalogItem.Description,
                    codeUnitMeasurement = x.CatalogItem.CodeUnitMeasurement,
                    unitMeasurement = x.CatalogItem.UnitMeasurement,
                    type = x.CatalogItem.Type,
                    value = x.Value,
                    quantity = x.Quantity,
                    total = x.Total,
                    comment = x.Comment,
                    classifier = x.ClassifierId.HasValue ? x.ClassifierId.Value : Guid.Empty,
                    activity = x.CatalogActivityId.HasValue ? x.CatalogActivityId.Value : Guid.Empty,
                    idGoal = _context.CatalogItemGoals.Include(y => y.CatalogGoal).Where(y => y.CatalogItemId == x.CatalogItem.Id && y.UserRequirementId == id).FirstOrDefault().CatalogGoal.SecFunc
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

        public async Task<UserRequirementItem> GetByUserRequrimentId(Guid userRequirementId)
            => await _context.UserRequirementItems.Where(x => x.UserRequirementId == userRequirementId).FirstOrDefaultAsync();

        public async Task<object> GetByUserRequrimentDetailId(Guid userRequirementId)
        {
            var userRequirementItem = await _context.UserRequirementItems.Include(x => x.CatalogItem).Where(x => x.UserRequirementId == userRequirementId).FirstOrDefaultAsync();

            var result = new
            {
                code = userRequirementItem.CatalogItem.Code,
                codeUnitMeasurement = userRequirementItem.CatalogItem.CodeUnitMeasurement,
                description = userRequirementItem.CatalogItem.Description,
                unitMeasurement = userRequirementItem.CatalogItem.UnitMeasurement,
                type = userRequirementItem.CatalogItem.Type,
                quantity = userRequirementItem.Quantity,
                value = userRequirementItem.Value,
                comment = userRequirementItem.Comment
            };

            return result;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
