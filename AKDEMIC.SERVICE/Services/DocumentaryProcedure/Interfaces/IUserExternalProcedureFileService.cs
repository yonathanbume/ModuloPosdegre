using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureFileService
    {
        Task<UserExternalProcedureFile> Get(Guid id);
        Task Delete(UserExternalProcedureFile userExternalProcedureFile);
        Task DeleteRange(IEnumerable<UserExternalProcedureFile> userExternalProcedureFiles);
        Task Insert(UserExternalProcedureFile userExternalProcedureFile);
        Task InsertRange(IEnumerable<UserExternalProcedureFile> userExternalProcedureFiles);
        Task<DataTablesStructs.ReturnedData<object>> GetRequerimentDocuments(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<List<UserExternalProcedureFile>> GetExternalProcedureFilesByUserExternalProcedure(Guid id);
    }
}
