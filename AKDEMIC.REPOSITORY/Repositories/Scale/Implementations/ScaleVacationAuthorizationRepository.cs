using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleVacationAuthorizationRepository : Repository<ScaleVacationAuthorization>, IScaleVacationAuthorizationRepository
    {
        public ScaleVacationAuthorizationRepository(AkdemicContext context) : base(context) { }

        public async Task<int> GetVacationAuthorizationsQuantity(string userId)
        {
            var records = await _context.ScaleVacationAuthorizations
                .Where(x => x.UserId == userId)
                .CountAsync();

            return records;
        }

        public async Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            var query = _context.ScaleVacationAuthorizations
                .Where(x => x.UserId == userId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Reason) : query.OrderBy(q => q.Reason);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleVacationAuthorization
                {
                    Id = x.Id,
                    ResolutionNumber = x.ResolutionNumber,
                    Reason = x.Reason,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionDocument = x.ResolutionDocument
                }).ToListAsync();

            return pagedList;
        }

        public async Task<Tuple<int, List<ScaleVacationAuthorization>>> GetVacationAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null)
        {
            var query = _context.ScaleVacationAuthorizations
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Reason.Contains(paginationParameter.SearchValue) ||
                            x.ResolutionNumber.Contains(paginationParameter.SearchValue) || x.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                            x.User.MaternalSurname.Contains(paginationParameter.SearchValue) || x.User.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicDepartmentDirectorId == userId).Select(x => x.Id).ToListAsync();
                    var teachers = _context.Teachers.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value)).Select(x => x.UserId);
                    query = query.Where(x => teachers.Contains(x.UserId));
                }
            }

            var records = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.User.PaternalSurname) : query.OrderBy(q => q.User.PaternalSurname);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Reason) : query.OrderBy(q => q.Reason);
                    break;
                case "3":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleVacationAuthorization
                {
                    Id = x.Id,
                    ResolutionNumber = x.ResolutionNumber,
                    Reason = x.Reason,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionDocument = x.ResolutionDocument,
                    User = new ApplicationUser
                    {
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        Name = x.User.Name,
                        FullName = x.User.FullName
                    }
                }).ToListAsync();

            return new Tuple<int, List<ScaleVacationAuthorization>>(records, pagedList);
        }

        public async Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsReport(string search, ClaimsPrincipal user = null)
        {
            var query =  _context.ScaleVacationAuthorizations
                .Include(x => x.User)
                .Include(x => x.LicenseResolutionType)
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Reason.Contains(search) ||
                            x.ResolutionNumber.Contains(search) || x.User.PaternalSurname.Contains(search) ||
                            x.User.MaternalSurname.Contains(search) || x.User.Name.Contains(search))
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicDepartmentDirectorId == userId).Select(x => x.Id).ToListAsync();
                    var teachers = _context.Teachers.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value)).Select(x => x.UserId);
                    query = query.Where(x => teachers.Contains(x.UserId));
                }
            }

            return await query.ToListAsync();
        }
    }
}
