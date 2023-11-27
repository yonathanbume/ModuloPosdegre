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
    public class RequirementRepository : Repository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<Requirement, dynamic>> GetRequirementsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.UserId);
                case "1":
                    return ((x) => x.UserId);
                default:
                    return ((x) => x.UserId);
            }
        }

        private Func<Requirement, string[]> GetRequirementsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Description + "",
                x.Subject + "",
                x.User.FullName + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<Requirement>> GetRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Requirement, Requirement>> selectPredicate = null, Expression<Func<Requirement, dynamic>> orderByPredicate = null, Func<Requirement, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Requirements
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Requirement>> GetRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetRequirementsDatatable(sentParameters, ExpressionHelpers.SelectRequirement(), GetRequirementsDatatableOrderByPredicate(sentParameters), GetRequirementsDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementOrderDatatable(DataTablesStructs.SentParameters sentParameters,int userRequirementIndex, string searchValue = null, int? filterValue = null)
        {
            var uit = await _context.UITs.OrderByDescending(e => e.Id).FirstOrDefaultAsync();

            var query = _context.UserRequirements.Include(x => x.Requirement)
                    .Where(x => x.OrderId != null)
                    .AsNoTracking();

            Expression<Func<UserRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Order.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Order.Title);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Order.Status);
                    break;
                default:
                    orderByPredicate = ((x) => x.Order.EndDate);
                    break;
            }


            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Order.Title.ToUpper().Contains(searchValue) || 
                                    x.Order.Code.ToUpper().Contains(searchValue) ||
                                    _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault().ToUpper().Contains(searchValue));
            

            if (filterValue == 1)
            {
                query = query.Where(x => x.Order.Cost != null && ((x.Order.Cost / uit.Value) < 8));
            }
            if (filterValue == 2)
            {
                query = query.Where(x => x.Order.Cost != null && ((x.Order.Cost / uit.Value) > 8));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    requerimentId = x.Requirement.Id,
                    x.Order.Code,
                    x.Order.Title,
                    item = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault(),
                    StatusId = x.Order.Status,
                    Status = ConstantHelpers.ORDERS.STATUS.VALUES.ContainsKey(x.Order.Status) == false
                            ? "Desconocido"
                            : ConstantHelpers.ORDERS.STATUS.VALUES[x.Order.Status],
                    x.Order.Description,
                    UserRequirementIndex = userRequirementIndex,
                    x.Order.Cost
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

        public async Task<List<Requirement>> GetList(Guid id)
            => await _context.Requirements.Where(x => x.Id == id).ToListAsync();

        public async Task<UserRequirement> GetWithIncludes(Guid id)
        {
            var query = _context.UserRequirements
                .Include(x => x.Requirement)
                    .ThenInclude(x => x.Supplier)
                .Include(x => x.Order)
                .Include(x => x.Requirement)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.UserDependencies)
                    .ThenInclude(x => x.Dependency)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByDependencyAndOrderTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            Expression<Func<UserRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Requirement.Code);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.UserRequirements.Include(x => x.Order).Include(x => x.Requirement.Dependency).Where(x => x.OrderId != null).AsNoTracking();
            
            if (centerId.HasValue && centerId != Guid.Empty)
                query = query.Where(x => x.Requirement.DependencyId == centerId);

            if (type.HasValue)
                query = query.Where(x => x.Order.Type == (type == 1 ? true : false));

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(x => startDate <= x.Order.StartDate && x.Order.StartDate <= endDate);
            }

            var recordsFiltered = await query.CountAsync();
            query = query.AsQueryable();

            var query2 = await query
                    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(x => new {
                        x.Order.Type,
                        x.Requirement.Dependency.Name,
                        x.Cost,
                    }).ToListAsync();

            var query3 = query2
                    .GroupBy(x => new { x.Type, x.Name })
                    .Select(x => new
                    {
                        dependency = x.Key.Name,
                        type = ConstantHelpers.ORDERS.TYPE.VALUES[x.Key.Type],
                        totalCost = x.Sum(y => y.Cost)
                    })
                    .ToList();

            var data = query3;
                
            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            { 
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetRequirementReportByDependencyAndOrderTypeChart(Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.UserRequirements.Include(x => x.Order).Include(x => x.Requirement.Dependency).Where(x => x.OrderId != null).AsNoTracking();

            if (centerId.HasValue && centerId != Guid.Empty)
                query = query.Where(x => x.Requirement.DependencyId == centerId);

            if (type.HasValue)
                query = query.Where(x => x.Order.Type == (type == 1 ? true : false));

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(x => startDate <= x.Order.StartDate && x.Order.StartDate <= endDate);
            }

            var query2 = await query
                .Include(x=>x.Order)
                .Include(x => x.Requirement)
                .Where(x => x.OrderId != null)
                .Select(x => new {
                    x.Order.Type,
                    x.Requirement.Dependency.Name,
                    x.Requirement.DependencyId,
                    x.Cost
                })
                .ToListAsync();

            var result = query2
                .GroupBy(x => new { x.Type, x.Name, x.DependencyId })
                .Select(x => new
                {
                    typeId = x.Key.Type,
                    dependencyId = x.Key.DependencyId,
                    dependency = x.Key.Name,
                    type = ConstantHelpers.ORDERS.TYPE.VALUES[x.Key.Type],
                    totalCost = x.Sum(y => y.Cost)
                });

            return result.ToList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByFundingSourceAndDependencyDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<UserRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Requirement.Code);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = await _context.UserRequirements.Include(x => x.Requirement)
                    .Where(x => x.OrderId != null)
                    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                    .Select(x=>new {
                        x.Order.FundingSource,
                        x.Requirement.Dependency.Name,
                        x.Cost
                    })
                    .ToListAsync();

            var recordsFiltered =  query.Count();

            var data =  query
                .GroupBy(x => new { x.FundingSource, x.Name })
                .Select(x => new
                {
                    dependency = x.Key.Name,
                    fundingSource = ConstantHelpers.ORDERS.FUNDING_SOURCE.VALUES[x.Key.FundingSource],
                    totalCost = x.Sum(y => y.Cost)
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetRequirementReportByFundingSourceAndDependencyChart()
        {
            var query = _context.UserRequirements.Include(x => x.Requirement).Where(x => x.OrderId != null).AsQueryable();

            var result = await query
                .Select(x => new 
                {
                    fundingSourceId = x.Order.FundingSource,
                    dependencyId = x.Requirement.DependencyId,
                    dependency = x.Requirement.Dependency.Name,
                    fundingSource = x.Order.FundingSource,
                    cost = x.Order.Cost
                })
                .Distinct()
                .GroupBy(x => new { x.fundingSourceId, x.dependency, x.dependencyId, x.fundingSource })
                .Select(x => new
                {
                    fundingSourceId = x.Key.fundingSourceId,
                    dependencyId = x.Key.dependencyId,
                    dependency = x.Key.dependency,
                    fundingSource = ConstantHelpers.ORDERS.FUNDING_SOURCE.VALUES[x.Key.fundingSource],
                    totalCost = x.Sum(y => y.cost)
                }).ToListAsync();

            return result;
        }

        public async Task<List<UserRequirement>> GetOrderDetailIncludeOrderUser(Guid id)
        {
            var result = await _context.UserRequirements
                .Include(x => x.Requirement).ThenInclude(x => x.Supplier)
                .Include(x => x.Order)
                .Include(x => x.Requirement).ThenInclude(x => x.User)
                .ThenInclude(x => x.UserDependencies)
                .ThenInclude(x => x.Dependency)
                           .Where(x => x.OrderId == id).ToListAsync();

            return result;
        }

        public async Task<Requirement> GetRequirementWithDataById(Guid id)
        {
            var requirement = await _context.Requirements.Include(x => x.UserRequirements).Where(x => x.UserRequirements.Any(s => s.Id == id)).FirstOrDefaultAsync();

            return requirement;
        }

        public async Task<object> GetRequirementDetail(Guid id)
        {
            var entity = await _context.Requirements.Where(x => x.UserRequirements.Any(s => s.Id == id))
                .Select(x => new
                {
                    x.Description,
                    x.Need,
                    x.Subject,
                    x.Folio
                }).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> AnyAsync(string codeNumber)
            => await _context.Requirements.AnyAsync(s => s.CodeNumber == codeNumber);

        public async Task<bool> FindByCodeNumberAndCodeNumber(string codeNumber, string codeNumber2)
            => await _context.Requirements.Where(x => x.CodeNumber != codeNumber).AnyAsync(s => s.CodeNumber == codeNumber2);
        #endregion
    }
}
