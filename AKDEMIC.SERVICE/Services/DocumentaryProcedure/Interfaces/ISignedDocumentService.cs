using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface ISignedDocumentService
    {
        Task Insert(SignedDocument document);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetSignedDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null);
        Task<SignedDocument> GetUnsignedDocument(string userId);
        Task<SignedDocument> Get(Guid id);
        Task Update(SignedDocument document);
    }
}
