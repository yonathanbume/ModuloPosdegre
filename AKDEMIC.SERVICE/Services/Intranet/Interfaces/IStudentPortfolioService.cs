using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentPortfolioService
    {
        Task<object> GetStudentPortfolioDatatable(Guid studentId, Guid? dependencyId = null, byte? type = null, bool? canUploadStudent = null, bool? onlyPending = null, ClaimsPrincipal user = null);
        Task Insert(StudentPortfolio entity);
        Task Update(StudentPortfolio entity);
        Task<StudentPortfolio> Get(Guid id, Guid type);
        Task<List<StudentPortfolio>> GetStudentPortfoliosByStudent(Guid studentId);
    }
}
