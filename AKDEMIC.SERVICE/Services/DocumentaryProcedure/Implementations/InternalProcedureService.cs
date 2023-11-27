using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class InternalProcedureService : IInternalProcedureService
    {
        private readonly IInternalProcedureRepository _internalProcedureRepository;

        public InternalProcedureService(IInternalProcedureRepository internalProcedureRepository)
        {
            _internalProcedureRepository = internalProcedureRepository;
        }

        public async Task<int> Count()
        {
            return await _internalProcedureRepository.Count();
        }

        public async Task<int> CountParentInternalProcedures(Guid id)
        {
            return await _internalProcedureRepository.CountParentInternalProcedures(id);
        }

        public async Task<int> CountYearlyInternalProceduresByDocumentType(Guid documentTypeId)
        {
            return await _internalProcedureRepository.CountYearlyInternalProceduresByDocumentType(documentTypeId);
        }

        public async Task<InternalProcedure> FirstSearchTreeInternalProcedureBySearchTree(int searchTree)
        {
            return await _internalProcedureRepository.FirstSearchTreeInternalProcedureBySearchTree(searchTree);
        }

        public async Task<InternalProcedure> LastSearchTreeInternalProcedureBySearchTree(int searchTree)
        {
            return await _internalProcedureRepository.LastSearchTreeInternalProcedureBySearchTree(searchTree);
        }

        public async Task<InternalProcedure> LastSearchTreeInternalProcedure()
        {
            return await _internalProcedureRepository.LastSearchTreeInternalProcedure();
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId)
        {
            return await _internalProcedureRepository.LastYearlyInternalProcedureByDependency(dependencyId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId, Guid documentTypeId)
        {
            return await _internalProcedureRepository.LastYearlyInternalProcedureByDependency(dependencyId, documentTypeId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId)
        {
            return await _internalProcedureRepository.LastYearlyInternalProcedureByDocumentType(documentTypeId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId, Guid dependencyId)
        {
            return await _internalProcedureRepository.LastYearlyInternalProcedureByDocumentType(documentTypeId, dependencyId);
        }

        public async Task<InternalProcedure> Get(Guid id)
        {
            return await _internalProcedureRepository.Get(id);
        }

        public async Task<InternalProcedure> GetInternalProcedure(Guid id)
        {
            return await _internalProcedureRepository.GetInternalProcedure(id);
        }

        public async Task<InternalProcedure> GetInternalProcedureByUserExternalProcedure(Guid userExternalProcedureId)
        {
            return await _internalProcedureRepository.GetInternalProcedureByUserExternalProcedure(userExternalProcedureId);
        }

        public async Task<IEnumerable<InternalProcedure>> GetChildInternalProcedures(Guid id)
        {
            return await _internalProcedureRepository.GetChildInternalProcedures(id);
        }

        public async Task<IEnumerable<InternalProcedure>> GetChildInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId)
        {
            return await _internalProcedureRepository.GetChildInternalProceduresByUserExternalProcedure(userExternalProcedureId);
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProcedures()
        {
            return await _internalProcedureRepository.GetDraftInternalProcedures();
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureRepository.GetDraftInternalProceduresDatatable(sentParameters, searchValuePredicate);
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresByUser(string userId)
        {
            return await _internalProcedureRepository.GetDraftInternalProceduresByUser(userId);
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureRepository.GetDraftInternalProceduresDatatableByUser(sentParameters, userId, searchValuePredicate);
        }

        public async Task<IEnumerable<InternalProcedure>> GetParentInternalProcedures(Guid id)
        {
            return await _internalProcedureRepository.GetParentInternalProcedures(id);
        }

        public async Task<IEnumerable<InternalProcedure>> GetParentInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId)
        {
            return await _internalProcedureRepository.GetParentInternalProceduresByUserExternalProcedure(userExternalProcedureId);
        }

        public async Task<IEnumerable<InternalProcedure>> GetPreviewInternalProcedures(Guid id)
        {
            return await _internalProcedureRepository.GetPreviewInternalProcedures(id);
        }

        public async Task<IEnumerable<InternalProcedure>> GetRemainingInternalProcedures(Guid id)
        {
            return await _internalProcedureRepository.GetRemainingInternalProcedures(id);
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProcedures()
        {
            return await _internalProcedureRepository.GetSentInternalProcedures();
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureRepository.GetSentInternalProceduresDatatable(sentParameters, searchValuePredicate);
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresByUser(string userId)
        {
            return await _internalProcedureRepository.GetSentInternalProceduresByUser(userId);
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureRepository.GetSentInternalProceduresDatatableByUser(sentParameters, userId, searchValuePredicate);
        }

        public async Task<IEnumerable<InternalProcedure>> GetInternalProceduresByUser(string userId)
        {
            return await _internalProcedureRepository.GetInternalProceduresByUser(userId);
        }

        public async Task Delete(InternalProcedure internalProcedure) =>
            await _internalProcedureRepository.Delete(internalProcedure);

        public async Task Insert(InternalProcedure internalProcedure) =>
            await _internalProcedureRepository.Insert(internalProcedure);

        public async Task InsertInternalProcedure(InternalProcedure internalProcedure)
        {
            await _internalProcedureRepository.InsertInternalProcedure(internalProcedure);
        }

        public async Task Update(InternalProcedure internalProcedure) =>
            await _internalProcedureRepository.Update(internalProcedure);

        public async Task<string> GetNextLastYearlyInternalCodeByDependencyAndDocumentType(Guid dependencyId, Guid documentTypeId)
            => await _internalProcedureRepository.GetNextLastYearlyInternalCodeByDependencyAndDocumentType(dependencyId, documentTypeId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToAcademicRecordByUserDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search = null, Guid? careerId = null, string startdate = null, string enddate = null, int? type = null, bool? onlyObserved = null,int? status = null)
            => await _internalProcedureRepository.GetInternalProceduresToAcademicRecordByUserDatatable(sentParameters, user, search,careerId,startdate,enddate,type,onlyObserved,status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToStudentbyUserId(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _internalProcedureRepository.GetInternalProceduresToStudentbyUserId(sentParameters, userId);

        public async Task<int> GetInternalProcedureCountByMonthAndUserId(int month, string userId)
            => await _internalProcedureRepository.GetInternalProcedureCountByMonthAndUserId(month, userId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalAndInternalProcedure(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _internalProcedureRepository.GetExternalAndInternalProcedure(sentParameters, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDraftInternalProceduresDatatableToManagement(DataTablesStructs.SentParameters parameters, string userId, string search)
            => await _internalProcedureRepository.GetDraftInternalProceduresDatatableToManagement(parameters, userId, search);

    }
}
