using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationDocumentService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search);
        Task Insert(ConvocationDocument entity);
        Task Update(ConvocationDocument entity);
        Task<IEnumerable<ConvocationDocument>> GetDocuments(Guid convocationId);
        Task Delete(ConvocationDocument entity);
        Task<ConvocationDocument> Get(Guid id);
    }
}
