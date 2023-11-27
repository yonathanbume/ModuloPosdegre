using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class CurrentAccountRepository : Repository<CurrentAccount>, ICurrentAccountRepository
    {
        public CurrentAccountRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurrentAccountsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<CurrentAccount, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    break;
            }

            var query = _context.CurrentAccounts.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    date = x.CreatedAt.ToLocalDateFormat()
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

        public async Task<IEnumerable<Select2Structs.Result>> GettCurrentAccountsSelect2ClientSide()
        {
            var result = await _context.CurrentAccounts
                          .Select(x => new Select2Structs.Result
                          {
                              Id = x.Id,
                              Text = x.Name
                          })
                          .OrderBy(x => x.Text)
                          .ToArrayAsync();

            return result;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
        {
            Expression<Func<CurrentAccount, dynamic>> orderByPredicate = null;

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
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            if (type.HasValue && type != 0)
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH)); break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH); break;
                    default:
                        break;
                }
            }

            var payments = await qryPayments
                .Select(x => new {
                    x.CurrentAccountId,
                    ConceptCurrentAccountId = x.Concept.CurrentAccountId,
                    x.Total
                })
                .ToListAsync();

            var query = _context.CurrentAccounts
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));

            var currentAccounts = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                }).ToListAsync();

            var data = currentAccounts
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    totalAmount = payments.Where(y => y.CurrentAccountId == x.Id || y.ConceptCurrentAccountId == x.Id).Sum(y => y.Total)
                }).ToList();

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
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

    }
}
