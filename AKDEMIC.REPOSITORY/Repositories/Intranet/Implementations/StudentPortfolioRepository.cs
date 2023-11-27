using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentPortfolioRepository : Repository<StudentPortfolio>, IStudentPortfolioRepository
    {
        public StudentPortfolioRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetStudentPortfolioDatatable(Guid studentId, Guid? dependencyId = null, byte? type = null, bool? canUploadStudent = null, bool? onlyPending = null, ClaimsPrincipal user = null)
        {

            var query = _context.StudentPortfolioTypes.AsNoTracking();

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            if (canUploadStudent.HasValue)
                query = query.Where(x => x.CanUploadStudent == canUploadStudent);

            var portfolios = await query.ToListAsync();

            var studentPortfolios = await _context.StudentPortfolios
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            Guid? currentDependencyId = null;

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var userClaimsDB = await _context.UserClaims.Where(x => x.UserId == userId).ToListAsync();
                    var userDependencyId = userClaimsDB.FirstOrDefault(x => x.ClaimType == ConstantHelpers.CLAIMS_USER.DEPENDENCY_ID).ClaimValue;
                    currentDependencyId = Guid.Parse(userDependencyId);
                }
            }

            var data = portfolios
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    received = studentPortfolios.Any(y => y.StudentPortfolioTypeId == x.Id && !string.IsNullOrEmpty(y.File)),
                    file = studentPortfolios.Any(y => y.StudentPortfolioTypeId == x.Id && !string.IsNullOrEmpty(y.File))
                    ? studentPortfolios.FirstOrDefault(y => y.StudentPortfolioTypeId == x.Id).File
                    : null,
                    validated = studentPortfolios.Any(y => y.StudentPortfolioTypeId == x.Id && y.IsValidated),
                    dependencyCanUploadFile = x.DependencyId == currentDependencyId
                })
                .ToList();

            if (onlyPending.HasValue)
            {
                if (onlyPending.Value)
                    data = data.Where(x => x.file == null).ToList();
                else
                    data = data.Where(x => x.file != null).ToList();
            }

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<List<StudentPortfolio>> GetStudentPortfoliosByStudent(Guid studentId)
            => await _context.StudentPortfolios.Where(x => x.StudentId == studentId).ToListAsync();
    }
}
