using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DocumentFormat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDocumentFormatService
    {
        Task<DocumentFormatTemplate> GetParsedDocumentFormat(byte recordType, Guid studentId, Guid? termId = null);
        Task Insert(AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat entity);
        Task Update(AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat entity);
        Task Delete(AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat entity);
        Task<AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat> Get(byte id);
        Task<DataTablesStructs.ReturnedData<object>> GetDocumentFormatsDatatable(DataTablesStructs.SentParameters parameters);
        Task<bool> AnyByRecordType(byte type);
    }
}
