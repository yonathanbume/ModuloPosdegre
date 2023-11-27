using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Concept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptRepository : IRepository<Concept>
    {
        Task<IEnumerable<Concept>> GetConceptsWithUser();
        Task<IEnumerable<Concept>> GetAllWithIncludes();
        Task<object> GetConceptsJson(string term);
        Task<object> GetConceptsWithAccountingPlanJson(string term, ClaimsPrincipal user = null);
        Task<bool> HasPayments(Guid id);
        Task<Concept> GetWithIncludes(Guid id);
        Task Update();

        Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null);
        Task<Concept> GetConceptIncludeLoadByConcept(string concept);
        Task<List<Concept>> GetConceptListByDependecyId(Guid id);
        Task<List<Concept>> GetConceptsByClassifierId(Guid id);
        IQueryable<Concept> ConceptsQry();
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null);
        Task<decimal> GetConceptoByDocument(Guid? id = null);

        Task<DataTablesStructs.ReturnedData<object>> GetConectpsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllConceptsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetUserConcepts(string userId, string term);
        Task<decimal> GetUnitaryAmount(Guid id);
        Task<object> GetReportBudgetBalance(Guid id);
        Task UpdateExtemporaneousEnrollmentConcept(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search = null, string code = null, byte? status = null, Guid? classifierId = null, Guid? dependencyId = null, bool? showTaxedOnly = null);
        Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user);
        Task<Select2Structs.ResponseParameters> GetConceptSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<bool> AnyWithCodeAndDescription(string code, string description);
        Task<List<Concept>> GetEnrollmentRelatedConcepts();
        Task<DataTablesStructs.ReturnedData<object>> GetConceptReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0);
        Task<decimal> GetConceptReportTotalAmount(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0);
        Task<List<ConceptReportTemplate>> GetConceptReportExcelData(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0);
        Task<List<PaymentTemplate>> GetPaymentsByConceptId(Guid id, DateTime? startDate = null, DateTime? endDate = null, byte type = 0);
    }
}
