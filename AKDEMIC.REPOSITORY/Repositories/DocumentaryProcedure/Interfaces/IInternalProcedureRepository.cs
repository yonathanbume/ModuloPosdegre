using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IInternalProcedureRepository : IRepository<InternalProcedure>
    {
        Task<int> CountParentInternalProcedures(Guid id);
        Task<int> CountYearlyInternalProceduresByDocumentType(Guid documentTypeId);
        Task<string> GetNextLastYearlyInternalCodeByDependencyAndDocumentType(Guid dependencyId, Guid documentTypeId); // delete later
        Task<InternalProcedure> FirstSearchTreeInternalProcedureBySearchTree(int searchTree);
        Task<InternalProcedure> LastSearchTreeInternalProcedureBySearchTree(int searchTree);
        Task<InternalProcedure> LastSearchTreeInternalProcedure();
        Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId);
        Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId, Guid documentTypeId);
        Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId);
        Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId, Guid dependencyId);
        Task<InternalProcedure> GetInternalProcedure(Guid id);
        Task<InternalProcedure> GetInternalProcedureByUserExternalProcedure(Guid userExternalProcedureId);
        Task<IEnumerable<InternalProcedure>> GetChildInternalProcedures(Guid id);
        Task<IEnumerable<InternalProcedure>> GetChildInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId);
        Task<IEnumerable<InternalProcedure>> GetDraftInternalProcedures();
        Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null);
        Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresByUser(string userId);
        Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null);
        Task<IEnumerable<InternalProcedure>> GetParentInternalProcedures(Guid id);
        Task<IEnumerable<InternalProcedure>> GetParentInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId);
        Task<IEnumerable<InternalProcedure>> GetPreviewInternalProcedures(Guid id);
        Task<IEnumerable<InternalProcedure>> GetSentInternalProcedures();
        Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null);
        Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresByUser(string userId);
        Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null);
        Task<IEnumerable<InternalProcedure>> GetRemainingInternalProcedures(Guid id);
        Task<IEnumerable<InternalProcedure>> GetInternalProceduresByUser(string userId);
        Task InsertInternalProcedure(InternalProcedure internalProcedure);
        Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToAcademicRecordByUserDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search = null, Guid? careerId = null, string startdate = null, string enddate = null, int? type = null, bool? onlyObserved = null,int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToStudentbyUserId(DataTablesStructs.SentParameters sentParameters, string userId);
        Task<int> GetInternalProcedureCountByMonthAndUserId(int month, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetExternalAndInternalProcedure(DataTablesStructs.SentParameters sentParameters, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetDraftInternalProceduresDatatableToManagement(DataTablesStructs.SentParameters parameters, string userId, string search);
    }
}
