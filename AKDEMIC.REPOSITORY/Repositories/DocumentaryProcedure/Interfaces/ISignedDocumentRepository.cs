using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface ISignedDocumentRepository : IRepository<SignedDocument>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSignedDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null);
        Task<SignedDocument> GetUnsignedDocument(string userId);
    }
}
