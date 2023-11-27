using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureRecordDocumentService
    {
        Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Delete(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument);
        Task Insert(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument);
        Task InsertRange(IEnumerable<UserExternalProcedureRecordDocument> userExternalProcedureRecordDocuments);
        Task Update(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument);
        Task<DataTablesStructs.ReturnedData<object>> GetUserExternalProcedureRecordDocumentsDatatableByuserExternalProcedureRecord(DataTablesStructs.SentParameters sentParameters, Guid userExternalProcedureRecordId);

        Task<List<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsByuserExternalProcedureRecord(Guid userExternalProcedureRecordId);








        Task<Tuple<int, List<UserExternalProcedureRecordDocument>>> GetUserExternalProcedureRecordDocuments(Guid userExternalProcedureRecordId, DataTablesStructs.SentParameters sentParameters);
    }
}
