using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Concept;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using Org.BouncyCastle.Ocsp;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptService : IConceptService
    {
        private readonly IConceptRepository _conceptRepository;

        public ConceptService(IConceptRepository conceptRepository)
        {
            _conceptRepository = conceptRepository;
        }

        public async Task<Concept> GetConcept(Guid id) => await _conceptRepository.Get(id);

        public async Task InsertRange(List<Concept> concepts)
            => await _conceptRepository.InsertRange(concepts);
        public async Task AddRange(List<Concept> concepts)
            => await _conceptRepository.AddRange(concepts);
        public async Task InsertConcept(Concept concept) => await _conceptRepository.Insert(concept);

        public async Task UpdateConcept(Concept concept) => await _conceptRepository.Update(concept);

        public async Task DeleteConcept(Concept concept) => await _conceptRepository.Delete(concept);

        public async Task<IEnumerable<Concept>> GetConcepts() => await _conceptRepository.GetAll();

        public async Task<IEnumerable<Concept>> GetConceptsWithUser() => await _conceptRepository.GetConceptsWithUser();
        public async Task<object> GetConceptsJson(string term)
            => await _conceptRepository.GetConceptsJson(term);
        
        public async Task<object> GetConceptsWithAccountingPlanJson(string term, ClaimsPrincipal user = null)
            => await _conceptRepository.GetConceptsWithAccountingPlanJson(term, user);

        public Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null) 
            => _conceptRepository.GetExtemporaneousEnrollmentDatatable(user);

        public async Task<Concept> GetConceptIncludeLoadByConcept(string concept)
            => await _conceptRepository.GetConceptIncludeLoadByConcept(concept);

        public async Task<List<Concept>> GetConceptListByDependecyId(Guid id)
            => await _conceptRepository.GetConceptListByDependecyId(id);

        public async Task<List<Concept>> GetConceptsByClassifierId(Guid id)
            => await _conceptRepository.GetConceptsByClassifierId(id);

        public IQueryable<Concept> ConceptsQry()
            =>  _conceptRepository.ConceptsQry();
        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null)
            => await _conceptRepository.GetPurchaseOrderDatatable(sentParameters, dependencyId);
        public async Task<decimal> GetConceptoByDocument(Guid? id = null)
            => await _conceptRepository.GetConceptoByDocument(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConectpsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string search)
            => await _conceptRepository.GetConectpsDatatable(sentParameters, userId, search);
        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null)
            => await _conceptRepository.GetConceptDatatable(sentParameters, user, search);
        public async Task<object> GetUserConcepts(string userId, string term)
            => await _conceptRepository.GetUserConcepts(userId, term);
        public async Task<decimal> GetUnitaryAmount(Guid id)
            => await _conceptRepository.GetUnitaryAmount(id);

        public async Task<Concept> GetWithIncludes(Guid id)
            => await _conceptRepository.GetWithIncludes(id);

        public async Task<bool> HasPayments(Guid id)
            => await _conceptRepository.HasPayments(id);
        public async Task<object> GetReportBudgetBalance(Guid id)
            => await _conceptRepository.GetReportBudgetBalance(id);

        public async Task UpdateExtemporaneousEnrollmentConcept(Guid studentId) => await _conceptRepository.UpdateExtemporaneousEnrollmentConcept(studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null, string code = null, byte? status = null, Guid? classifierId = null, Guid? dependencyId = null, bool? showTaxedOnly = null)
            => await _conceptRepository.GetConceptsDatatable(sentParameters, user, search, code, status, classifierId, dependencyId, showTaxedOnly);

        public async Task<IEnumerable<Concept>> GetAllWithIncludes()
            => await _conceptRepository.GetAllWithIncludes();

        public async Task<Concept> Add(Concept concept)
            => await _conceptRepository.Add(concept);

        public async Task Update()
            => await _conceptRepository.Update();

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user)
            => await _conceptRepository.GetExtemporaneousEnrollmentDatatableServerSide(sentParameters, user);
        public async Task<Select2Structs.ResponseParameters> GetConceptSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => await _conceptRepository.GetConceptSelect2(requestParameters, searchValue);

        public async Task<bool> AnyWithCodeAndDescription(string code, string description)
        {
            return await _conceptRepository.AnyWithCodeAndDescription(code, description);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetAllConceptsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => _conceptRepository.GetAllConceptsDatatable(sentParameters,search);

        public async Task<List<Concept>> GetEnrollmentRelatedConcepts()
            => await _conceptRepository.GetEnrollmentRelatedConcepts();

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
            => await _conceptRepository.GetConceptReportDatatable(sentParameters, startDate, endDate, user, search, type);

        public async Task<decimal> GetConceptReportTotalAmount(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
            => await _conceptRepository.GetConceptReportTotalAmount(startDate, endDate, user, search, type);

        public async Task<List<ConceptReportTemplate>> GetConceptReportExcelData(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
            => await _conceptRepository.GetConceptReportExcelData(startDate, endDate, user, search, type);

        public async Task<List<PaymentTemplate>> GetPaymentsByConceptId(Guid id, DateTime? startDate = null, DateTime? endDate = null, byte type = 0)
            => await _conceptRepository.GetPaymentsByConceptId(id, startDate, endDate, type);

    }
}
