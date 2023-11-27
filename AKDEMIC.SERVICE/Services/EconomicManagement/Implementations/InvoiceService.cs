using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Invoice;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<IEnumerable<Invoice>> GetAllByUser(string userId, bool? isCanceled = null)
            => await _invoiceRepository.GetAllByUser(userId, isCanceled);

        public async Task<Invoice> GetInvoice(Guid id) => await _invoiceRepository.Get(id);

        public async Task InsertInvoice(Invoice invoice) => await _invoiceRepository.Insert(invoice);
        public async Task<object> GetPayedDebts(string userId)
            => await _invoiceRepository.GetPayedDebts(userId);
        public async Task<List<Invoice>> GetInvoiceList(Guid id)
            => await _invoiceRepository.GetInvoiceList(id);
        public async Task<object> GetInvoiceByUserIdDatetime(string userId, DateTime _startDateTime, DateTime _endDateTime)
            => await _invoiceRepository.GetInvoiceByUserIdDatetime(userId, _startDateTime, _endDateTime);
        public IQueryable<Invoice> GetInvoiceByUserId(string userId)
            => _invoiceRepository.GetInvoiceByUserId(userId);
        public IQueryable<Invoice> GetInvoiceByUserIdAndSunatStatus(string userId, byte? status = null)
            => _invoiceRepository.GetInvoiceByUserIdAndSunatStatus(userId, status);
        public async Task<Invoice> GetInclude(Guid id)
            => await _invoiceRepository.GetInclude(id);
        public async Task<List<Invoice>> GetBySerie(string series)
            => await _invoiceRepository.GetBySerie(series);
        public async Task AddAsync(Invoice invoice)
            => await _invoiceRepository.Add(invoice);
        public async Task<Invoice> GetInvoiceCanceledById(Guid id, Guid pettycashId)
            => await _invoiceRepository.GetInvoiceCanceledById(id, pettycashId);
        public async Task<Tuple<bool, string>> AnnulInvoice(Guid id, string observation = null)
            => await _invoiceRepository.AnnulInvoice(id, observation);

        public async Task<object> GetDailyInvoicesByUser(Guid pettyCashBookId, byte? invoicePaymentType = null)
            => await _invoiceRepository.GetDailyInvoicesByUser(pettyCashBookId, invoicePaymentType);

        public async Task Update(Invoice invoice)
        {
            await _invoiceRepository.Update(invoice);
        }

        public async Task<Invoice> GetByData(DateTime date, string series, int receipt, decimal amount)
        {
            return await _invoiceRepository.GetByData(date, series, receipt, amount);
        }

        public async Task<object> GetElectronicDocumentDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, decimal? amount = null)
            => await _invoiceRepository.GetElectronicDocumentDatatable(sentParameters, startDate, endDate, amount);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAnnulledInvoiceReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
            => await _invoiceRepository.GetAnnulledInvoiceReportDatatable(sentParameters, startDate, endDate, userId);

        public async Task<List<AnnulledInvoiceTemplate>> GetAnnulledInvoiceReportData(DateTime? startDate = null, DateTime? endDate = null, string userId = null)
            => await _invoiceRepository.GetAnnulledInvoiceReportData(startDate, endDate, userId);

    }
}
