using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentPortfolioRepository : IRepository<StudentPortfolio>
    {
        Task<object> GetStudentPortfolioDatatable(Guid studentId, Guid? dependencyId = null, byte? type = null, bool? canUploadStudent = null, bool? onlyPending = null, ClaimsPrincipal user = null);
        Task<List<StudentPortfolio>> GetStudentPortfoliosByStudent(Guid studentId);
    }
}
