using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserExternalProcedureFileRepository : IRepository<UserExternalProcedureFile>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetRequerimentDocuments(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<List<UserExternalProcedureFile>> GetExternalProcedureFilesByUserExternalProcedure(Guid id);
    }
}
