using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DocumentFormat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDocumentFormatRepository : IRepository<AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat>
    {
        Task<bool> AnyByRecordType(byte type);
        Task<DataTablesStructs.ReturnedData<object>> GetDocumentFormatsDatatable(DataTablesStructs.SentParameters parameters);
        Task<DocumentFormatTemplate> GetParsedDocumentFormat(byte recordType, Guid studentId, Guid? termId = null);
    }
}
