using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class PettyCashRepository : Repository<PettyCash>, IPettyCashRepository
    {
        public PettyCashRepository(AkdemicContext context) : base(context) { }

        public async Task<PettyCash> GetUserPettyCash(string id) =>
            await _context.PettyCash.Where(x => x.UserId == id && !x.Closed).FirstOrDefaultAsync();

        public async Task<PettyCash> GetUserPettyCashWithSerialNumber(string id) =>
            await _context.PettyCash.Where(x => x.UserId == id && !x.Closed)
                .Select(x => new PettyCash
                {
                    Id = x.Id,
                    InitialAmount = x.InitialAmount,
                    InitialDate = x.InitialDate
                }).FirstOrDefaultAsync();

        public async Task<int> GetInvoiceCount(Guid id) => await _context.PettyCash.Where(x => x.Id == id).Select(x => x.Invoices.Count).FirstOrDefaultAsync();

        public async Task<bool> HasPettyCash(string id) => await _context.PettyCash.AnyAsync(pc => !pc.Closed && pc.UserId == id);

        public async Task<IEnumerable<Invoice>> GetPettyCashInvoices(Guid id) => await _context.Invoices.IgnoreQueryFilters().Include(x => x.Payments).ThenInclude(x => x.Concept).ThenInclude(x => x.Classifier).Where(x => x.PettyCashId == id && x.Canceled).ToListAsync();

        public async Task<IEnumerable<Invoice>> GetCurrentPettyCashInvoices(string id)
        {
            var pettyCashId = (await _context.PettyCash.FirstOrDefaultAsync(pc => !pc.Closed && pc.UserId == id)).Id;

            return await _context.Invoices
                .Where(x => x.PettyCashId == pettyCashId && x.Canceled)
                .OrderByDescending(x=>x.Number)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAmount(Guid id) => await _context.PettyCash.Where(x => x.Id == id)
            .Select(x => x.Invoices.Where(y => !y.Annulled).Sum(y => y.TotalAmount)).FirstOrDefaultAsync();

        public async Task<IEnumerable<PettyCash>> GetUserPettyCashes(string userId)
        {
            return await _context.PettyCash.Where(x => x.UserId == userId && x.Closed).Include(x => x.Invoices).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInvoiceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue = null)
        {
            Expression<Func<Invoice, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Number;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ClientName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.TotalAmount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Annulled;
                    break;
                default:
                    orderByPredicate = (x) => x.Date;
                    break;
            }


            var query = _context.Invoices.Where(x => x.PettyCashId == id && x.Canceled)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var trimValue = searchValue.Trim().ToUpper();

                query = query.Where(x => x.ClientName.ToUpper().Contains(trimValue));
            }

            var recordsFiltered = await query.CountAsync();
            
            query = query.AsQueryable();

            if (sentParameters.RecordsPerDraw == 0)
            {
                var data1 = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .OrderBy(x => x.Number)
               .Select(x => new
               {
                   id = x.Id,
                   number = $"{x.Series}-{x.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}",
                   client = x.ClientName,
                   amount = x.TotalAmount,
                   state = x.Annulled,
               }).ToListAsync();

                var recordsTotal1 = data1.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data1,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal1
                };
            }

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .OrderBy(x => x.Number)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    number = $"{x.Series}-{x.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}",
                    client = x.ClientName,
                    amount = x.TotalAmount,
                    state = x.Annulled,
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
        public async Task<List<PettyCash>> GetReportPaymentByDate(string date)
        {
            var pettycashes = await _context.PettyCash.Include(x => x.Invoices).Include(x => x.User)
                 .Where(x => x.InitialDate.ToString("dd/MM/yyyy").Equals(date)).ToListAsync();

            return pettycashes;
        }

        public async Task<PettyCash> GetPettyCashByClosedAndUserId(string userId)
            => await _context.PettyCash.Where(pc => !pc.Closed && pc.UserId == userId).FirstAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserPettyCashesDatatable(DataTablesStructs.SentParameters parameters, string userId, DateTime? date = null)
        {
            Expression<Func<PettyCash, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.InitialDate;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Invoices.Where(y=>y.Canceled).Count();
                    break;
                case "2":
                    orderByPredicate = (x) => x.Invoices.Where(y => y.Annulled).Count();
                    break;
                case "3":
                    orderByPredicate = (x) => x.InitialAmount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.AmountCollected;
                    break;
                case "5":
                    orderByPredicate = (x) => x.DeclaredAmount;
                    break;
                default:
                    orderByPredicate = (x) => x.InitialDate;
                    break;
            }

            var query = _context.PettyCash.Where(x => x.Closed && x.UserId == userId).AsNoTracking();

            if(date != null)
            {
                //query = query.Where(x=>x.InitialDate.)
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    date = x.InitialDate.ToLocalDateFormat(),
                    initialAmount = $"S/.{x.InitialAmount:0.00}",
                    amountCollected = $"S/.{x.AmountCollected:0.00}",
                    declaredAmount = $"S/.{x.DeclaredAmount}",
                    invoices = x.Invoices.Where(y=>y.Canceled).Count(),
                    annulled = x.Invoices.Where(y=>y.Annulled).Count()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

    }
}
