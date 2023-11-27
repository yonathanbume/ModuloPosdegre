using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureRecordDocumentRepository : IRepository<UserExternalProcedureRecordDocument>
    {
        Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserExternalProcedureRecordDocumentsDatatableByuserExternalProcedureRecord(DataTablesStructs.SentParameters sentParameters, Guid userExternalProcedureRecordId);
        Task<List<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsByuserExternalProcedureRecord(Guid userExternalProcedureRecordId);





        Task<Tuple<int, List<UserExternalProcedureRecordDocument>>> GetUserExternalProcedureRecordDocuments(Guid userExternalProcedureRecordId, DataTablesStructs.SentParameters sentParameters);
    }
}
