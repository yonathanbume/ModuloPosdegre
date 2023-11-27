using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureFileService
    {
        Task<UserProcedureFile> Get(Guid id);
        Task Delete(UserProcedureFile userProcedureFile);
        Task DeleteRange(IEnumerable<UserProcedureFile> userProcedureFiles);
        Task Insert(UserProcedureFile userProcedureFile);
        Task InsertRange(IEnumerable<UserProcedureFile> userProcedureFiles);
        Task<List<UserProcedureFile>> GetUserProcedureFiles(Guid userProcedureId);
        Task Update(UserProcedureFile userProcedureFile);
    }
}
