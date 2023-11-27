using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class AccountingPlanRepository : Repository<AccountingPlan>, IAccountingPlanRepository
    {
        public AccountingPlanRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAccountingPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<AccountingPlan, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    //orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.AccountingPlans.AsQueryable();

            //if (!string.IsNullOrEmpty(search))
            //    query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) || x.Code.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                }, search)
                .CountAsync();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.Id,
                   x.Code,
                   x.Name
               }, search).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.AccountingPlans.AnyAsync(x => x.Name == name && x.Id != ignoredId);

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _context.AccountingPlans.AnyAsync(x => x.Code == code && x.Id != ignoredId);

        public async Task<IEnumerable<Select2Structs.Result>> GetAccountingPlansSelect2ClientSide()
        {
            var result = await _context.AccountingPlans
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"[{x.Code}] {x.Name}"
                })
                .ToArrayAsync();

            return result;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false)
        {
            Expression<Func<AccountingPlan, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    break;
                default:
                    //orderByPredicate = (x) => x.Code;
                    break;
            }

            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            var payments = await qryPayments
                .Select(x => new
                {
                    x.Concept.AccountingPlanId,
                    x.Total
                })
                .ToListAsync();

            var query = _context.AccountingPlans
                .AsNoTracking();

            //var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));

            var accountingPlans = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                }).ToListAsync();

            var data = accountingPlans
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    totalAmount = payments.Where(y => y.AccountingPlanId == x.Id).Sum(y => y.Total)
                }).ToList();

            if (!showAll) data = data.Where(x => x.totalAmount > 0).ToList();

            if (sentParameters.OrderColumn == "2")
            {
                if (sentParameters.OrderColumn == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                    data = data.OrderByDescending(x => x.totalAmount).ToList();
                else
                    data = data.OrderBy(x => x.totalAmount).ToList();
            }

            data = data
                 //.Skip(sentParameters.PagingFirstRecord)
                 //.Take(sentParameters.RecordsPerDraw)
                 .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

    }
}
