using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string number = null, string matter = null, Guid? dependencyId = null, string resolutionDate = null, Guid? sorterId = null, Guid? categoryId = null,string startDate = null, string endDate = null, string searchValue = null, ClaimsPrincipal user = null, Guid? facultyId = null, int? year = null, byte? status = null, byte? type = null)
            => await _documentRepository.GetDocumentsDatatable(sentParameters, number, matter, dependencyId, resolutionDate, sorterId,categoryId, startDate, endDate, searchValue, user, facultyId, year, status , type);

        public async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatableByUserId(DataTablesStructs.SentParameters sentParameters, string userId, string numberOfAct = null, string matter = null, Guid? dependencyId = null, string date = null, string searchValue = null)
            => await _documentRepository.GetDocumentsDatatableByUserId(sentParameters, userId, numberOfAct, matter, dependencyId, date, searchValue);

        public async Task InsertAsync(Document document)
            => await _documentRepository.Insert(document);

        public async Task DeleteAsync(Document document)
            => await _documentRepository.Delete(document);

        public async Task UpdateAsync(Document document)
            => await _documentRepository.Update(document);

        public async Task<Document> GetAsync(Guid documentId)
            => await _documentRepository.Get(documentId);

        public async Task<IEnumerable<Select2Structs.Result>> GetDependencies()
            => await _documentRepository.GetDependencies();

        public async Task<Dependency> GetDependencyById(Guid dependencyId)
            => await _documentRepository.GetDependencyById(dependencyId);

        public async Task<IEnumerable<Tuple<int, int>>> GetReportByDependencyId(Guid? dependencyId, Guid? facultyId = null)
            => await _documentRepository.GetReportByDependencyId(dependencyId, facultyId);

        public async Task<IEnumerable<Select2Structs.Result>> GetDependenciesByUserId(string userId)
            => await _documentRepository.GetDependenciesByUserId(userId);

        public async Task<bool> AnyByNumber(string number, Guid? ignoredId = null)
            => await _documentRepository.AnyByNumber(number, ignoredId);

        public async Task<bool> AnyByResolutionCategoryId(Guid resolutionCategoryId)
            => await _documentRepository.AnyByResolutionCategoryId(resolutionCategoryId);

        public async Task<bool> AnyBySorterId(Guid sorterId)
            => await _documentRepository.AnyBySorterId(sorterId);
    }
}
