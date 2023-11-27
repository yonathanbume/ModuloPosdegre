using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Invoice;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> GetInvoice(Guid id);

        Task InsertInvoice(Invoice invoice);

        Task<IEnumerable<Invoice>> GetAllByUser(string userId, bool? isCanceled = null);
        Task<object> GetPayedDebts(string userId);
        Task<List<Invoice>> GetInvoiceList(Guid id);
        Task<object> GetInvoiceByUserIdDatetime(string userId, DateTime _startDateTime, DateTime _endDateTime);
        IQueryable<Invoice> GetInvoiceByUserId(string userId);
        IQueryable<Invoice> GetInvoiceByUserIdAndSunatStatus(string userId, byte? status = null);
        Task<Invoice> GetInclude(Guid id);
        Task<List<Invoice>> GetBySerie(string series);
        Task AddAsync(Invoice invoice);
        Task<Invoice> GetInvoiceCanceledById(Guid id, Guid pettycashId);
        Task<Tuple<bool, string>> AnnulInvoice(Guid id, string observation = null);
        Task<object> GetDailyInvoicesByUser(Guid pettyCashBookId, byte? invoicePaymentType = null);
        Task<Invoice> GetByData(DateTime date, string series, int receipt, decimal amount);
        Task Update(Invoice invoice);
        Task<object> GetElectronicDocumentDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, decimal? amount = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAnnulledInvoiceReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, string userId = null);
        Task<List<AnnulledInvoiceTemplate>> GetAnnulledInvoiceReportData(DateTime? startDate = null, DateTime? endDate = null, string userId = null);
    }
}
