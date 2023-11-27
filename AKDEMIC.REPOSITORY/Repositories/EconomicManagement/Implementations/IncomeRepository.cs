using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Income;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class IncomeRepository : Repository<Income>, IIncomeRepository
    {
        public IncomeRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Income>> GetAllByDependency(Guid dependencyId)
        {
            var incomes = await _context.Incomes
                .Where(x => x.DependencyId == dependencyId)
                .ToListAsync();

            return incomes;
        }

        public async Task<List<Income>> GetByInvoiceId(Guid invoiceId)
        {

            var incomes = await _context.Incomes
                .Where(x => x.PaymentId.HasValue && x.Payment.InvoiceId == invoiceId)
                .ToListAsync();

            return incomes;
        }

        public async Task<DataTablesStructs.ReturnedData<DailyIncomeTemplate>> GetDailyIncomeDatatableClientSide(DateTime startDate, DateTime endDate, Guid? dependencyId = null)
        {
            startDate = DateTime.SpecifyKind(startDate,DateTimeKind.Unspecified).ToUtcDateTime();
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Unspecified).AddDays(1).AddTicks(-1).ToUtcDateTime();

            var paymentsQry = _context.Payments.Where(x => x.PaymentDate.HasValue && startDate <= x.PaymentDate.Value && x.PaymentDate.Value <= endDate).AsNoTracking();
            var incomesQry = _context.Incomes.Where(x => startDate <= x.Date && x.Date <= endDate).AsNoTracking();

            var expensesQry = _context.Expenses.Where(x => startDate <= x.Date && x.Date <= endDate).AsNoTracking();
            var provisionsQry = _context.ExpenditureProvisions.Where(x => x.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING && startDate <= x.CreatedAt.Value && x.CreatedAt.Value <= endDate).AsNoTracking();// await _expenditureProvisionService.ProvisionsQry(datetime.Date, );
            var transfersQry = _context.BalanceTransfers.Where(x => startDate <= x.CreatedAt.Value && x.CreatedAt.Value <= endDate).AsNoTracking();

            if (dependencyId.HasValue && dependencyId.Value != Guid.Empty)
            {
                paymentsQry = paymentsQry.Where(x => x.Concept.DependencyId == dependencyId);
                incomesQry = incomesQry.Where(x => x.DependencyId == dependencyId);

                expensesQry = expensesQry.Where(x => x.DependencyId == dependencyId);
                provisionsQry = provisionsQry.Where(x => x.DependencyId == dependencyId);
            }

            var incomes = !dependencyId.HasValue ? await paymentsQry
                 .Select(x => new DailyIncomeTemplate
                 {
                     date = x.PaymentDate.ToLocalDateFormat(),
                     siaf = "",
                     invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number}" : "",
                     document = "",
                     order = "",
                     concept = x.Description,
                     month = "",
                     provision = 0.00M,
                     income = x.Type == ConstantHelpers.Treasury.Income.Type.INCOME ? x.Total : 0.00M,
                     expense = x.Type == ConstantHelpers.Treasury.Income.Type.OUTCOME ? x.Total : 0.00M
                 })
                 .ToListAsync()
                 : await incomesQry.Select(x => new DailyIncomeTemplate
                 {
                     date = x.Date.ToLocalDateFormat(),
                     siaf = "",
                     invoice = "",
                     document = "",
                     order = "",
                     concept = x.Description,
                     month = "",
                     provision = 0.00M,
                     income = x.Amount,
                     expense = 0.00M
                 })
                 .ToListAsync();


            var expenses = await expensesQry
                .Select(x => new DailyIncomeTemplate
                {
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = x.Invoice,
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    month = $"{x.Date:MM/yyyy}",
                    provision = 0.00M,
                    income = 0.00M,
                    expense = x.Amount
                })
                .ToListAsync();

            var provision = await provisionsQry
                .Select(x => new DailyIncomeTemplate
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

            var data = incomes;

            data.AddRange(expenses);

            data.AddRange(provision);

            if (dependencyId.HasValue && dependencyId.Value != Guid.Empty)
            {
                var fromTransfers = await _context.BalanceTransfers
                    .Where(x => x.FromDependencyId == dependencyId && startDate <= x.CreatedAt.Value.Date && x.CreatedAt.Value.Date <= endDate)
                    .Select(x => new DailyIncomeTemplate
                    {
                        date = x.CreatedAt.ToLocalDateFormat(),
                        siaf = "",
                        invoice = "",
                        document = "",
                        order = "",
                        concept = $"Transferencia de Balance -> {x.ToDependency.Name}",
                        month = "",
                        provision = 0.00M,
                        income = 0.00M,
                        expense = x.Amount * -1.0M
                    })
                 .ToListAsync();
                data.AddRange(fromTransfers);

                var toTransfers = await _context.BalanceTransfers
                   .Where(x => x.ToDependencyId == dependencyId && startDate <= x.CreatedAt.Value.Date && x.CreatedAt.Value.Date <= endDate)
                   .Select(x => new DailyIncomeTemplate
                   {
                       date = x.CreatedAt.ToLocalDateFormat(),
                       siaf = "",
                       invoice = "",
                       document = "",
                       order = "",
                       concept = $"Transferencia de Balance <- {x.FromDependency.Name}",
                       month = "",
                       provision = 0.00M,
                       income = x.Amount,
                       expense = 0.00M
                   })
                .ToListAsync();
                data.AddRange(toTransfers);
            }

            data = data.OrderBy(x => x.date).ToList();

            return new DataTablesStructs.ReturnedData<DailyIncomeTemplate>
            {
                DrawCounter = 1,//ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                RecordsTotal = data.Count,
                RecordsFiltered = data.Count,
                Data = data
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncomesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters, int year ,Guid? dependencyId = null )
        {
            var query = _context.Incomes
                .Where(x => x.Date.Year == year)
                .AsQueryable();

            if (dependencyId != null)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    date = x.Date.ToLocalDateFormat(),
                    invoice = x.Invoice,
                    concept = x.Description,
                    type = x.Type,
                    amount = x.Type == ConstantHelpers.Treasury.Income.Type.OUTCOME ? (x.Amount * -1 ): x.Amount
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

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
