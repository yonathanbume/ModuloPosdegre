using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRecordDocumentService
    {
        Task InsertRange(IEnumerable<UserProcedureRecordDocument> documents);
        Task<Tuple<int, List<UserProcedureRecordDocument>>> GetUserProcedureRecordDocuments(Guid userProcedureRecordId, DataTablesStructs.SentParameters sentParameters);
    }
}
