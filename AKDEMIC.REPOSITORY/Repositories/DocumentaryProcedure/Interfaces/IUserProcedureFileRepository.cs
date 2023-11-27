using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserProcedureFileRepository : IRepository<UserProcedureFile>
    {
        Task<List<UserProcedureFile>> GetUserProcedureFiles(Guid userProcedureId);
    }
}
