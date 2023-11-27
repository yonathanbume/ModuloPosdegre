using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ExpenditureProvisionRepository : Repository<ExpenditureProvision>, IExpenditureProvisionRepository
    {

        public ExpenditureProvisionRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ExpenditureProvision, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.ExpenditureProvisions
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Concept.Contains(search));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    //id = x.Id,
                    concept = x.Concept,
                    amount = x.Amount,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    dependency = x.Dependency.Name,
                    status = x.Status
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

        public async Task<object> GetProvisionByDependencyAndStatus(Guid id, int status)
        {
            var provision = await _context.ExpenditureProvisions
                .Where(x =>
                x.DependencyId == id &&
                x.Status == status)
                .Select(x => new
                {
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = x.Concept,
                    month = "",
                    provision = x.Amount,
                    income = 0.00M,
                    expense = 0.00M
                }).ToListAsync();

            return provision;
        }

        public async Task<List<ExpenditureProvision>> GetExpenditureProvisionList(Guid id, int status)
        {
            var provision = await _context.ExpenditureProvisions
                .Where(x =>
                x.DependencyId == id &&
                x.Status == status).ToListAsync();

            return provision;
        }

        public async Task<List<ExpenditureProvision>> GetExpenditureProvisionStatusList(int status)
        {
            var provision = await _context.ExpenditureProvisions
                .Where(x =>
                x.Status == status).ToListAsync();

            return provision;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableDependency(string userId, DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ExpenditureProvision, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var userDependencies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();

            var query = _context.ExpenditureProvisions
                .Where(x => userDependencies.Contains(x.DependencyId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();
            
            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Concept.ToUpper().Contains(search.ToUpper()));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    concept = x.Concept,
                    amount = x.Amount,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    dependency = x.Dependency.Name,
                    status = x.Status,
                    month = $"{x.Date:MM/yyyy}",
                    code = x.Code,
                    document = x.ReferenceDocument,
                    order = x.Order
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

        public IQueryable<ExpenditureProvision> ProvisionsQry(DateTime date, int status)
            => _context.ExpenditureProvisions.Where(x => x.CreatedAt.Value.Date == date
            && x.Status == status).AsQueryable();

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableProvision(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ExpenditureProvision, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.ExpenditureProvisions
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Concept.ToUpper().Contains(search.ToUpper()));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    concept = x.Concept,
                    amount = x.Amount,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    dependency = x.Dependency.Name,
                    status = x.Status,
                    code = x.Code,
                    document = x.ReferenceDocument,
                    order = x.Order,
                    month = $"{x.Date:MM/yyyy}"
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
    }
}
