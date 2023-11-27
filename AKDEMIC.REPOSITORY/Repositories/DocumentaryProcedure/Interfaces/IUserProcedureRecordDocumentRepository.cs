using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureRecordDocumentRepository : IRepository<UserProcedureRecordDocument>
    {
        Task<Tuple<int, List<UserProcedureRecordDocument>>> GetUserProcedureRecordDocuments(Guid userProcedureRecordId, DataTablesStructs.SentParameters sentParameters);
    }
}
