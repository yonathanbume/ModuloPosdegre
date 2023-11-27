using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Invoice;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Invoice>> GetAllByUser(string userId, bool? isCanceled = null)
        {
            var query = _context.Invoices
                .Where(x => x.Payments.Any(y => y.UserId == userId))
                .AsNoTracking();

            if (isCanceled.HasValue)
                query = query.Where(x => x.Canceled == isCanceled.Value);

            return await query.ToListAsync();
        }
        public async Task<object> GetPayedDebts(string userId)
        {
            var result = await _context.Invoices
            .Where(
                x => x.Payments.Any(y => y.UserId == userId)
                && x.Canceled
            )
            .Select(
                x => new
                {
                    id = x.Id,
                    number = $"{x.Series} - {x.Number:00000000}",
                    items = x.Payments.Count,
                    type = ConstantHelpers.Treasury.DocumentType.NAMES[x.DocumentType],
                    totalamount = x.TotalAmount,
                    discount = x.Descount,
                    paymentDate = x.Date.ToShortDateString()
                }
            )
            .ToListAsync();

            return result;
        }

        public async Task<List<Invoice>> GetInvoiceList(Guid id)
            => await _context.Invoices
            .Include(x => x.ExternalUser)
            .Where(i => i.Id == id)
            .ToListAsync();

        public async Task<object> GetInvoiceByUserIdDatetime(string userId, DateTime _startDateTime, DateTime _endDateTime)
        {
            var data = _context.Invoices
                .Where(x => x.PettyCash.UserId == userId && x.Canceled);

            data = data.Where(x => x.Date >= _startDateTime && x.Date <= _endDateTime);

            var result = await data.Select(x => new
            {
                issueDate = x.Date.ToLocalDateFormat(),
                type = x.DocumentType,
                number = $"{x.Series} - {x.Number:00000000}",
                client = x.ClientName,
                amount = x.TotalAmount,
                id = x.Id,
                sunatStatus = x.SunatStatus,
                cdrUrl = x.SunatCdrUrl,
                cancel_state = x.SunatStatus == 5 ? _context.CreditNotes.Where(y => y.InvoiceId == x.Id).Select(y => new
                {
                    y.Id,
                    y.SunatStatus,
                    y.SunatCdrUrl
                }).FirstOrDefault() : null
            }).ToListAsync();

            return result;
        }

        public IQueryable<Invoice> GetInvoiceByUserId(string userId)
            => _context.Invoices.Where(x => x.PettyCash.UserId == userId && x.Canceled);

        public IQueryable<Invoice> GetInvoiceByUserIdAndSunatStatus(string userId, byte? status = null)
            => _context.Invoices.Where(x => x.PettyCash.UserId == userId && x.Canceled && x.SunatStatus != status);
        public async Task<Invoice> GetInclude(Guid id)
            => await _context.Invoices.Include(x => x.ExternalUser)
            .Include(x => x.User).Include(x => x.InvoiceDetails).FirstOrDefaultAsync(x => x.Id == id);
        public async Task<List<Invoice>> GetBySerie(string series)
            => await _context.Invoices.Where(x => x.Series == series).ToListAsync();
        public async Task<Invoice> GetInvoiceCanceledById(Guid id, Guid pettycashId)
            => await _context.Invoices.Where(i => i.Id == id && i.Canceled && i.PettyCashId == pettycashId).FirstAsync();

        public async Task<Tuple<bool, string>> AnnulInvoice(Guid id, string observation = null)
        {
            var invoice = await _context.Invoices
                .Where(i => i.Id == id && i.Canceled && !i.PettyCash.Closed)
                .FirstOrDefaultAsync();

            if (invoice == null)
                return new Tuple<bool, string>(false, "No se ha encontrado el documento indicado");

            invoice.Annulled = true;
            invoice.TotalAmount = 0.0M;
            invoice.Subtotal = 0.0M;
            invoice.IgvAmount = 0.0M;

            if (!string.IsNullOrEmpty(observation))
                invoice.Comment = observation;
            
            var invoiceDetails = await _context.InvoiceDetails.Where(i => i.InvoiceId == invoice.Id).ToListAsync();
            foreach (var item in invoiceDetails)
            {
                item.Total = 0;
                item.SubTotal = 0;
                item.IgvAmount = 0;
            }

            var incomes = await _context.Incomes.Where(x => x.PaymentId.HasValue && x.Payment.InvoiceId == invoice.Id).ToListAsync();
            _context.Incomes.RemoveRange(incomes);

            var details = await _context.Payments.Where(i => i.InvoiceId == invoice.Id).ToListAsync();
            foreach (var item in details)
            {
                if (item.Type != ConstantHelpers.PAYMENT.TYPES.POSTGRADE_DATABASE)
                    item.InvoiceId = null;

                item.Status = ConstantHelpers.PAYMENT.STATUS.PENDING;
                item.PaymentDate = null;

                var userProcedure = await _context.UserProcedures.FirstOrDefaultAsync(x => x.Id == item.EntityId);
                if (userProcedure != null)
                {
                    if (userProcedure.Status != ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED && userProcedure.Status != ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT)
                        return new Tuple<bool, string>(false, "El documento esta relacionado a un trámite en proceso");
                    else
                    {
                        var files = await _context.UserProcedureFiles.Where(x => x.UserProcedureId == userProcedure.Id).ToListAsync();
                        _context.UserProcedureFiles.RemoveRange(files);
                        _context.UserProcedures.Remove(userProcedure);
                    }
                }

                if (item.ConceptId.HasValue)
                {
                    var concept = await _context.Concepts.FindAsync(item.ConceptId);
                    if (concept.Amount == 0)
                    {
                        item.Total = 0;
                        item.SubTotal = 0;
                        item.IgvAmount = 0;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        //DataTablesStructs.SentParameters sentParameters, 
        public async Task<object> GetDailyInvoicesByUser(Guid pettyCashBookId, byte? invoicePaymentType = null)
        {
            var book = await _context.PettyCashBooks.FindAsync(pettyCashBookId);

            var startDate = book.Date.Date.ToUtcDateTime();
            var endDate = book.Date.Date.AddDays(1).AddTicks(-1).ToUtcDateTime();

            var query = _context.InvoiceDetails
                .Where(x => startDate <= x.Invoice.PettyCash.InitialDate && x.Invoice.PettyCash.InitialDate <= endDate
                && x.Invoice.PettyCash.UserId == book.UserId && x.Invoice.Canceled)
                .AsNoTracking();

            if (invoicePaymentType.HasValue)
                query = query.Where(x => x.Invoice.PaymentType == invoicePaymentType);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    date = x.Invoice.Date.ToLocalDateFormat(),
                    datetime = x.Invoice.Date,
                    type = 1,
                    number = x.Invoice.Annulled ? $"{x.Invoice.Series}-{x.Invoice.Number:000000}-" : $"{x.Invoice.Series}-{x.Invoice.Number:000000}",
                    description = x.Invoice.Annulled ? $"ANULADO - {x.Invoice.ClientName}" : x.Invoice.ClientName,
                    income = x.Invoice.Annulled ? 0.00M : x.Total,
                    //outcome = 0.00M,
                    outcome = x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.VOUCHER ? x.Total : 0.00M,
                    account = x.ConceptId.HasValue ? x.Concept.AccountingPlan.Code : "",
                    accountId = x.ConceptId.HasValue ? x.Concept.AccountingPlanId : Guid.Empty,
                    annulled = x.Invoice.Annulled
                }).ToListAsync();

            var recordsTotal = data.Count;

            var data2 = await _context.BankDeposits
                .Where(x => x.PettyCashBookId == pettyCashBookId)
                 .Select(x => new
                 {
                     id = x.Id,
                     date = x.Date.ToLocalDateFormat(),
                     datetime = x.Date,
                     type = 2,
                     number = x.Number,
                     description = x.Description,
                     income = 0.00M,
                     outcome = x.Amount,
                     account = x.CurrentAccount.Name,
                     accountId = x.CurrentAccountId,
                     annulled = false
                 })
                .ToListAsync();

            data.AddRange(data2);

            data = data.OrderBy(x => x.datetime).ToList();
            return data;
        }

        public async Task<Invoice> GetByData(DateTime date, string series, int receipt, decimal amount)
        {
            return await _context.Invoices.Where(x => x.Series == series && x.Number == receipt && x.TotalAmount == amount).FirstOrDefaultAsync();
        }

        public async Task<object> GetElectronicDocumentDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, decimal? amount = null)
        {
            Expression<Func<Invoice, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ClientName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Number;
                    break;
                case "3":
                    orderByPredicate = (x) => x.TotalAmount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.SunatStatus;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Number;
                    break;
            }

            var internalSaleDocument = byte.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.EconomicManagement.INTERNAL_SALE_DOCUMENT_TYPE));

            var query = _context.Invoices
                .Where(x => x.Canceled && x.DocumentType == internalSaleDocument)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.Date >= startDate);
            if (endDate.HasValue) query = query.Where(x => x.Date <= endDate);

            if (amount.HasValue && amount > 0)
                query = query.Where(x => amount <= x.TotalAmount);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    issueDate = x.Date.ToLocalDateFormat(),
                    type = x.DocumentType,
                    number = $"{x.Series}-{x.Number:00000000}",
                    client = x.ClientName,
                    amount = x.TotalAmount,
                    sunatStatus = x.SunatStatus,
                    sunatType = x.ElectronicDocumentType.HasValue && ConstantHelpers.Treasury.ElectronicDocumentType.NAMES.ContainsKey(x.ElectronicDocumentType.Value) ? ConstantHelpers.Treasury.ElectronicDocumentType.NAMES[x.ElectronicDocumentType.Value] : "-",
                    sunatDocument = x.SunatTicket
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetAnnulledInvoiceReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
        {
            Expression<Func<Invoice, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Series;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ClientName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.TotalAmount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.PettyCash.User.FullName;
                    break;
                default:
                    break;
            }

            var query = _context.Invoices
                .Where(x => x.Annulled)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.Date <= endDate.Value);

            if (!string.IsNullOrEmpty(userId) && userId != Guid.Empty.ToString())
                query = query.Where(x => x.PettyCash.UserId == userId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
               //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .OrderByConditionThenBy(sentParameters.OrderDirection, orderByPredicate, sentParameters.OrderColumn == "0" ? (x) => x.Number : null)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   id = x.Id,
                   invoice = $"{x.Series}-{x.Number:00000000}",
                   date = x.Date.ToLocalDateFormat(),
                   client = x.ClientName,
                   total = x.TotalAmount,
                   cashier = x.PettyCash.User.FullName
               }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        
        public async Task<List<AnnulledInvoiceTemplate>> GetAnnulledInvoiceReportData(DateTime? startDate = null, DateTime? endDate = null, string userId = null)
        {
            var query = _context.Invoices
                .Where(x => x.Annulled)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.Date <= endDate.Value);

            if (!string.IsNullOrEmpty(userId) && userId != Guid.Empty.ToString())
                query = query.Where(x => x.PettyCash.UserId == userId);

            var data = await query
               .Select(x => new AnnulledInvoiceTemplate
               {
                   Id = x.Id,
                   Invoice = $"{x.Series}-{x.Number:00000000}",
                   Date = x.Date.ToLocalDateFormat(),
                   Client = x.ClientName,
                   Total = x.TotalAmount,
                   Cashier = x.PettyCash.User.FullName
               })
               .ToListAsync();

            return data;
        }

    }
}
