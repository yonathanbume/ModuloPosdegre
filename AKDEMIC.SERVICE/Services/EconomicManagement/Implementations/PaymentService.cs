using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> Get(Guid id)
        {
            return await _paymentRepository.Get(id);
        }

        public async Task<IEnumerable<Payment>> GetPendingPayments(string code) =>
            await _paymentRepository.GetPendingPayments(code);

        public async Task<IEnumerable<Payment>> GetPostulantPayments(Guid postulantId) =>
            await _paymentRepository.GetPostulantPayments(postulantId);

        public async Task<IEnumerable<Payment>> GetPendingExternalPayments(string code) =>
            await _paymentRepository.GetPendingExternalPayments(code);

        public async Task AddAsync(Payment payment)
            => await _paymentRepository.Add(payment);
        public async Task Delete(Payment payment) =>
            await _paymentRepository.Delete(payment);

        public async Task Insert(Payment payment) =>
            await _paymentRepository.Insert(payment);

        public async Task Update(Payment payment) =>
            await _paymentRepository.Update(payment);

        public async Task InsertRange(List<Payment> payments)
            => await _paymentRepository.InsertRange(payments);

        public async Task<IEnumerable<(int day, string accounting, decimal total)>> GetEducationalRateSummaryByYearAndMonth(int year, int month) =>
            await _paymentRepository.GetEducationalRateSummaryByYearAndMonth(year, month);

        public async Task<int> DegreePaymentCount(bool isIntegrated, List<Guid> resultsToSearch)
        {

            return await _paymentRepository.DegreePaymentCount(isIntegrated, resultsToSearch);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentsByConceptList(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _paymentRepository.GetPaymentsByConceptList(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> ConceptGenerateByRegistryPattern(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            return await _paymentRepository.ConceptGenerateByRegistryPattern(sentParameters, facultyId, careerId, selectPredicate, searchValuePredicate, searchValue);
        }

        public async Task<IEnumerable<Payment>> GetAllByUser(string userId, Guid termId)
            => await _paymentRepository.GetAllByUser(userId, termId);

        public async Task<IEnumerable<Payment>> GetAllByUser(string userId, byte? status = null, byte? type = null)
            => await _paymentRepository.GetAllByUser(userId, status, type);
        public async Task<object> GetPaymentsHome(string userId)
            => await _paymentRepository.GetPaymentsHome(userId);

        public async Task<bool> ExistsAnotherExtemporaneousPaymentForUser(string userId)
        {
            return await _paymentRepository.ExistsAnotherExtemporaneousPaymentForUser(userId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatablePayment(DataTablesStructs.SentParameters sentParameters, byte? status = null, bool onlyConfigs = false, string search = null)
            => await _paymentRepository.GetDatatablePayment(sentParameters, status, onlyConfigs, search);

        public async Task<List<Payment>> GetPaymentsReport(byte status, string date)
            => await _paymentRepository.GetPaymentsReport(status, date);

        public async Task<List<Payment>> GetPaymentIncludeUserByStatusAndInvoiceId(byte status, Guid invoiceId)
            => await _paymentRepository.GetPaymentIncludeUserByStatusAndInvoiceId(status, invoiceId);
        public IQueryable<Payment> GetPaymentQuery(List<Guid?> userProcedures)
            => _paymentRepository.GetPaymentQuery(userProcedures);
        public async Task<object> GetIncomes(List<Concept> concepts)
            => await _paymentRepository.GetIncomes(concepts);
        public async Task<List<Payment>> GetPaymentsAnyConcept(List<Concept> concepts)
            => await _paymentRepository.GetPaymentsAnyConcept(concepts);
        public async Task<object> GetPaymentReport(List<Concept> concepts)
            => await _paymentRepository.GetPaymentReport(concepts);
        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, string search)
            => await _paymentRepository.GetPaymentDatatableReport(sentParameters, search);
        public IQueryable<Payment> PaymentsQry(DateTime date)
            => _paymentRepository.PaymentsQry(date);
        public IQueryable<Payment> PaymentsQryConcept()
            => _paymentRepository.PaymentsQryConcept();
        public async Task<object> GetOutstandingDebts(string userId)
            => await _paymentRepository.GetOutstandingDebts(userId);
        public async Task<List<Payment>> GetdbPayments(Guid[] payments)
            => await _paymentRepository.GetdbPayments(payments);
        public async Task<object> GetPayedDebtDetail(Guid id)
            => await _paymentRepository.GetPayedDebtDetail(id);
        public async Task<List<Payment>> GetInvoiceDetailList(Guid id)
            => await _paymentRepository.GetInvoiceDetailList(id);
        public async Task<List<Payment>> GetPaymentByListConcept(IEnumerable<Concept> concepts)
            => await _paymentRepository.GetPaymentByListConcept(concepts);
        public async Task<List<Payment>> GetDetailsDocument(Guid id)
            => await _paymentRepository.GetDetailsDocument(id);
        public async Task<List<Payment>> GetPaymentsByIdList(Guid id)
            => await _paymentRepository.GetPaymentsByIdList(id);
        public async Task AddRangeAsync(List<Payment> payments)
            => await _paymentRepository.AddRange(payments);
        public async Task<List<Payment>> GetPaymentByListPaymentGuid(List<Guid> payments)
            => await _paymentRepository.GetPaymentByListPaymentGuid(payments);
        public async Task<List<Payment>> GetPaymentListByInvoiceId(Guid id)
            => await _paymentRepository.GetPaymentListByInvoiceId(id);
        public async Task<List<Payment>> GetPaymentWithDataById(Guid id)
            => await _paymentRepository.GetPaymentWithDataById(id);
        public async Task<List<Payment>> GetPaymentWithData()
            => await _paymentRepository.GetPaymentWithData();
        public async Task<List<Payment>> GetInvoiceByPettyCashId(Guid id)
            => await _paymentRepository.GetInvoiceByPettyCashId(id);
        public async Task<object> GetAssociatePaymentsGet(string userId, byte? status = null)
            => await _paymentRepository.GetAssociatePaymentsGet(userId, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBankPaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, Guid? formatId = null, ClaimsPrincipal user = null, string search = null)
            => await _paymentRepository.GetBankPaymentDatatableReport(sentParameters, startDate, endDate, formatId, user, search);

        public async Task<IEnumerable<Payment>> GetAllBankPaymentByDate(string issueDate)
            => await _paymentRepository.GetAllBankPaymentByDate(issueDate);

        public async Task<List<Payment>> GetPaymentWithDataByPettyCashIdClosed(Guid id)
            => await _paymentRepository.GetPaymentWithDataByPettyCashIdClosed(id);

        public async Task SaveChangesPayment()
        {
            await _paymentRepository.SaveChanges();
        }

        public async Task<bool> WereProceduresPaid(string userId)
        {
            return await _paymentRepository.WereProceduresPaid(userId);
        }

        public async Task<bool> WereOtherProceduresPaid(string userId)
            => await _paymentRepository.WereOtherProceduresPaid(userId);

        public void Remove(Payment payment)
        {
            _paymentRepository.Remove(payment);
        }
        public void RemoveRange(IEnumerable<Payment> payments)
        {
            _paymentRepository.RemoveRange(payments);
        }

        public async Task<List<Payment>> NotPaidProcedures()
        {
            return await _paymentRepository.NotPaidProcedures();
        }

        public async Task<List<Payment>> GetByUserProcedures(List<UserProcedure> userProcedures)
        {
            return await _paymentRepository.GetByUserProcedures(userProcedures);
        }

        public async Task CreateEnrollmentConceptsJob(Guid term)
        {
            await _paymentRepository.CreateEnrollmentConceptsJob(term);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetPartialPaymentDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => _paymentRepository.GetPartialPaymentDatatable(sentParameters, search);

        public Task<Payment> GetWithIncludes(Guid id)
            => _paymentRepository.GetWithIncludes(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPhantomPaymentsDataTable(DataTablesStructs.SentParameters sentParamters, string searchValue)
        {
            return await _paymentRepository.GetPhantomPaymentsDataTable(sentParamters, searchValue);
        }

        public async Task<List<Payment>> GetInvoiceByPettyCashIdV2(Guid id)
            => await _paymentRepository.GetInvoiceByPettyCashIdV2(id);

        public async Task<object> GetStudentEnrollmentPaymentsDatatable(Guid studentId) => await _paymentRepository.GetStudentEnrollmentPaymentsDatatable(studentId);

        public Task DeleteDirectedCourseStudentPayments(Guid directedCourseStudentId) => _paymentRepository.DeleteDirectedCourseStudentPayments(directedCourseStudentId);

        public Task InsertDirectedCourseStudentPayment(Guid directedCourseStudentId, string UserId) => _paymentRepository.InsertDirectedCourseStudentPayment(directedCourseStudentId, UserId);

        public async Task<IEnumerable<Payment>> GetAllBankPaymentsByDateRange(Guid formatId, string startDate, string endDate, ClaimsPrincipal user = null, string search = null)
            => await _paymentRepository.GetAllBankPaymentsByDateRange(formatId, startDate, endDate, user, search);

        public async Task<object> GetAllBankPaymentsTreasuryDatatable(Guid formatId, string startDate, string endDate, ClaimsPrincipal user = null, string search = null)
            => await _paymentRepository.GetAllBankPaymentsTreasuryDatatable(formatId, startDate, endDate, user, search);

        public async Task<IEnumerable<Payment>> GetAllBankPaymentsByDateRange(Guid formatId, string startDate, string endDate, Guid conceptId, ClaimsPrincipal user = null)
            => await _paymentRepository.GetAllBankPaymentsByDateRange(formatId, startDate, endDate, conceptId, user);

        public async Task<List<IncomeReceiptTemplate>> GetIncomeReceiptReportData(DateTime start, DateTime end, int? type = null)
            => await _paymentRepository.GetIncomeReceiptReportData(start, end, type);

        public async Task<DateTime?> GetLastBankBatchDate()
            => await _paymentRepository.GetLastBankBatchDate();

        public async Task<DateTime?> GetLastBankBatchDate(Guid formatId)
            => await _paymentRepository.GetLastBankBatchDate(formatId);

        public async Task<List<DateTime>> GetAllBankPaymentDates()
            => await _paymentRepository.GetAllBankPaymentDates();

        public async Task<List<DateTime>> GetAllBankPaymentDates(Guid formatId)
            => await _paymentRepository.GetAllBankPaymentDates(formatId);

        public async Task<decimal> GetBankPaymentTotalAmount(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null)
            => await _paymentRepository.GetBankPaymentTotalAmount(startDate, endDate, user, search);

        public async Task<List<Payment>> GetByPettyCashBookId(Guid pettyCashBookId)
            => await _paymentRepository.GetByPettyCashBookId(pettyCashBookId);

        public async Task<Payment> GetPhantomPaymentByVoucher(DateTime date, string voucher, decimal amount)
        {
            return await _paymentRepository.GetPhantomPaymentByVoucher(date, voucher, amount);
        }
        public async Task<List<Payment>> GetPhantomPaymentByDocument(string document)
        {
            return await _paymentRepository.GetPhantomPaymentByDocument(document);
        }
        public async Task<bool> AnyWithDateOperationAndTotal(DateTime datetime, string sequence, decimal amount)
            => await _paymentRepository.AnyWithDateOperationAndTotal(datetime, sequence, amount);
        public async
        Task<bool> AnyWithDateOperationAndTotalExternal(DateTime datetime, string sequence, decimal amount)
            => await _paymentRepository.AnyWithDateOperationAndTotalExternal(datetime, sequence, amount);
        public async Task<Payment> GetByDateOperationAndTotal(DateTime datetime, string sequence, decimal amount)
            => await _paymentRepository.GetByDateOperationAndTotal(datetime, sequence, amount);

        public async Task UpdateRange(IEnumerable<Payment> payments)
        {
            await _paymentRepository.UpdateRange(payments);
        }

        public async Task ValidateUsedPayments(List<Payment> payments)
            => await _paymentRepository.ValidateUsedPayments(payments);

        public async Task<Payment> GetPhantomPaymentByVoucher(string voucher, DateTime date)
            => await _paymentRepository.GetPhantomPaymentByVoucher(voucher, date);

        public Task<DataTablesStructs.ReturnedData<object>> GetPaymentByUserDatatable(DataTablesStructs.SentParameters sentParameters, string UserId, int? status = null, string searchValue = null)
            => _paymentRepository.GetPaymentByUserDatatable(sentParameters, UserId, status, searchValue);

        public async Task GenerateStudentReentryPayments(Guid studentId)
            => await _paymentRepository.GenerateStudentReentryPayments(studentId);

        public async Task<List<Payment>> GetPaymentsByUser(string userId, byte? status, Guid? conceptId)
            => await _paymentRepository.GetPaymentsByUser(userId, status, conceptId);

        public async Task UpdateStudentEnrollmentPayments(Guid studentId)
           => await _paymentRepository.UpdateStudentEnrollmentPayments(studentId);

        public Task<DataTablesStructs.ReturnedData<object>> GetExoneratedPaymentsToUserDatatable(DataTablesStructs.SentParameters sentParameters, string userName, string searchValue = null)
            => _paymentRepository.GetExoneratedPaymentsToUserDatatable(sentParameters, userName, searchValue);

        public async Task GenerateStudentEnrollmentPayments()
           => await _paymentRepository.GenerateStudentEnrollmentPayments();

        public async Task<List<Payment>> GetPaymentsByClientId(Guid clientId, byte? status)
            => await _paymentRepository.GetPaymentsByClientId(clientId, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBankLoadDatatableReport(int year)
           => await _paymentRepository.GetBankLoadDatatableReport(year);

        public async Task<object> GetBankLoadFullCalendar(int year, int month)
           => await _paymentRepository.GetBankLoadFullCalendar(year, month);

        public async Task<object> GetBankLoadYearsSelect2()
           => await _paymentRepository.GetBankLoadYearsSelect2();
        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptIncomePaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, string searchValue = null, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
            => await _paymentRepository.GetConceptIncomePaymentDatatableReport(sentParameters, searchValue, startDate, endDate, userId);
        public async Task<decimal> GetConceptIncomePaymentDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
            => await _paymentRepository.GetConceptIncomePaymentDatatableReportTotalAmount(search, startDate, endDate, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentPaymentDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, Guid? conceptId = null, ClaimsPrincipal user = null, byte? type = null, string search = null)
        => await _paymentRepository.GetStudentPaymentDatatable(sentParameters, startDate, endDate, conceptId, user, type, search);

        public async Task<List<StudentPaymentTemplate>> GetStudentPaymentData(DateTime? startDate = null, DateTime? endDate = null, Guid? conceptId = null, ClaimsPrincipal user = null, byte? type = null, string search = null)
        => await _paymentRepository.GetStudentPaymentData(startDate, endDate, conceptId, user, type, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentsDetailedByCashierDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate, DateTime? endDate, string cashierId)
            => await _paymentRepository.GetPaymentsDetailedByCashierDatatable(sentParameters, startDate, endDate, cashierId);

        public async Task<List<PaymentsDetailedTemplate>> GetPaymentsDetailedByCashierTemplate(DateTime? startDate, DateTime? endDate, string cashierId)
            => await _paymentRepository.GetPaymentsDetailedByCashierTemplate(startDate, endDate, cashierId);

        public async Task<bool> AnyByEntityId(Guid entityId)
            => await _paymentRepository.AnyByEntityId(entityId);

        public async Task<Payment> GetPaymentByOperationCodeToValidateProcedure(string userId, DateTime date, string operationCodeB, decimal amount, bool exactAmount = true)
            => await _paymentRepository.GetPaymentByOperationCodeToValidateProcedure(userId, date, operationCodeB, amount, exactAmount);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUnusedPaymentsDatatable(DataTablesStructs.SentParameters parameters, string userId, decimal? minAmount, Guid? conceptId = null)
            => await _paymentRepository.GetUnusedPaymentsDatatable(parameters, userId, minAmount, conceptId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentReportDatatableData(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, byte type = 0, Guid? formatId = null, string userId = null)
            => await _paymentRepository.GetPaymentReportDatatableData(sentParameters, startDate, endDate, type, formatId, userId);

        public async Task<List<PaymentReportTemplate>> GetPaymentReportData(DateTime? startDate = null, DateTime? endDate = null, byte type = 0, Guid? formatId = null, string userId = null)
            => await _paymentRepository.GetPaymentReportData(startDate, endDate, type, formatId, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExoneratedEnrollmentPaymentsDatatableData(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int? academicYear = null, string searchValue = null)
            => await _paymentRepository.GetExoneratedEnrollmentPaymentsDatatableData(sentParameters, termId, careerId, academicYear, searchValue);

        public async Task<List<StudentPaymentTemplate>> GetExoneratedEnrollmentPaymentsData(Guid termId, Guid? careerId = null, int? academicYear = null, string searchValue = null)
            => await _paymentRepository.GetExoneratedEnrollmentPaymentsData(termId, careerId, academicYear, searchValue);

        public async Task DeleteRange(List<Payment> payments)
            => await _paymentRepository.DeleteRange(payments);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExoneratedPaymentsDatatableData(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, byte? type = null, string searchValue = null)
            => await _paymentRepository.GetExoneratedPaymentsDatatableData(sentParameters, termId, careerId, type, searchValue);

        public async Task<List<StudentPaymentTemplate>> GetExoneratedPaymentsData(Guid termId, Guid? careerId = null, byte? type = null, string searchValue = null)
            => await _paymentRepository.GetExoneratedPaymentsData(termId, careerId, type, searchValue);

        public async Task<List<IncomeReceiptTemplate>> GetClassifierReportData(DateTime start, DateTime end, int? type = null, bool publicSector = false)
            => await _paymentRepository.GetClassifierReportData(start, end, type, publicSector);

        public async Task<IncomeReceiptTemplate> GetClassifierReportDataRange(DateTime start, DateTime end, int? type = null, bool publicSector = false)
            => await _paymentRepository.GetClassifierReportDataRange(start, end, type, publicSector);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId, Guid? admissionTypeId)
            => await _paymentRepository.GetPostulantPaymentDatatable(sentParameters, applicationTermId, careerId, admissionTypeId);

        public async Task<List<PostulantPaymentTemplate>> GetPostulantPaymentData(Guid applicationTermId, Guid? careerId, Guid? admissionTypeId)
            => await _paymentRepository.GetPostulantPaymentData(applicationTermId, careerId, admissionTypeId);

        public async Task<object> GetCashierDailyIncomeDatatableData(DateTime date, string userId)
            => await _paymentRepository.GetCashierDailyIncomeDatatableData(date, userId);

        public async Task<CashierDailyIncomeTemplate> GetCashierDailyIncomeData(DateTime date, string userId)
            => await _paymentRepository.GetCashierDailyIncomeData(date, userId);

    }
}